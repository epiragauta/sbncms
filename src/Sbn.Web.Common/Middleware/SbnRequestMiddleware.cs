using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Smidge.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.PublishedCache;
using Sbn.Cms.Infrastructure.WebAssets;
using Sbn.Cms.Web.Common.Profiler;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.Middleware
{

    /// <summary>
    /// Manages Sbn request objects and their lifetime
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is responsible for initializing the content cache
    /// </para>
    /// <para>
    /// This is responsible for creating and assigning an <see cref="ISbnContext"/>
    /// </para>
    /// </remarks>
    public class SbnRequestMiddleware : IMiddleware
    {
        private readonly ILogger<SbnRequestMiddleware> _logger;

        private readonly ISbnContextFactory _sbnContextFactory;
        private readonly IRequestCache _requestCache;
        private readonly PublishedSnapshotServiceEventHandler _publishedSnapshotServiceEventHandler;
        private readonly IEventAggregator _eventAggregator;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SbnRequestPaths _sbnRequestPaths;
        private readonly BackOfficeWebAssets _backOfficeWebAssets;
        private readonly IRuntimeState _runtimeState;
        private readonly IVariationContextAccessor _variationContextAccessor;
        private readonly IDefaultCultureAccessor _defaultCultureAccessor;
        private readonly SmidgeOptions _smidgeOptions;
        private readonly WebProfiler _profiler;

        private static bool s_cacheInitialized;
        private static bool s_cacheInitializedFlag = false;
        private static object s_cacheInitializedLock = new object();

#pragma warning disable IDE0044 // Add readonly modifier
        private static bool s_firstBackOfficeRequest;
        private static bool s_firstBackOfficeReqestFlag;
        private static object s_firstBackOfficeRequestLocker = new object();
#pragma warning restore IDE0044 // Add readonly modifier

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnRequestMiddleware"/> class.
        /// </summary>
        public SbnRequestMiddleware(
            ILogger<SbnRequestMiddleware> logger,
            ISbnContextFactory sbnContextFactory,
            IRequestCache requestCache,
            PublishedSnapshotServiceEventHandler publishedSnapshotServiceEventHandler,
            IEventAggregator eventAggregator,
            IProfiler profiler,
            IHostingEnvironment hostingEnvironment,
            SbnRequestPaths sbnRequestPaths,
            BackOfficeWebAssets backOfficeWebAssets,
            IOptions<SmidgeOptions> smidgeOptions,
            IRuntimeState runtimeState,
            IVariationContextAccessor variationContextAccessor,
            IDefaultCultureAccessor defaultCultureAccessor)
        {
            _logger = logger;
            _sbnContextFactory = sbnContextFactory;
            _requestCache = requestCache;
            _publishedSnapshotServiceEventHandler = publishedSnapshotServiceEventHandler;
            _eventAggregator = eventAggregator;
            _hostingEnvironment = hostingEnvironment;
            _sbnRequestPaths = sbnRequestPaths;
            _backOfficeWebAssets = backOfficeWebAssets;
            _runtimeState = runtimeState;
            _variationContextAccessor = variationContextAccessor;
            _defaultCultureAccessor = defaultCultureAccessor;
            _smidgeOptions = smidgeOptions.Value;
            _profiler = profiler as WebProfiler; // Ignore if not a WebProfiler
        }

        /// <inheritdoc/>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            // do not process if client-side request
            if (context.Request.IsClientSideRequest())
            {
                // we need this here because for bundle requests, these are 'client side' requests that we need to handle
                LazyInitializeBackOfficeServices(context.Request.Path);
                await next(context);
                return;
            }

            // Profiling start needs to be one of the first things that happens.
            // Also MiniProfiler.Current becomes null if it is handled by the event aggregator due to async/await
            _profiler?.SbnApplicationBeginRequest(context, _runtimeState.Level);

            EnsureContentCacheInitialized();

            _variationContextAccessor.VariationContext ??= new VariationContext(_defaultCultureAccessor.DefaultCulture);
            SbnContextReference sbnContextReference = _sbnContextFactory.EnsureSbnContext();

            Uri currentApplicationUrl = GetApplicationUrlFromCurrentRequest(context.Request);
            _hostingEnvironment.EnsureApplicationMainUrl(currentApplicationUrl);

            var pathAndQuery = context.Request.GetEncodedPathAndQuery();

            try
            {
                // Verbose log start of every request
                LogHttpRequest.TryGetCurrentHttpRequestId(out Guid httpRequestId, _requestCache);
                _logger.LogTrace("Begin request [{HttpRequestId}]: {RequestUrl}", httpRequestId, pathAndQuery);

                try
                {
                    LazyInitializeBackOfficeServices(context.Request.Path);
                    await _eventAggregator.PublishAsync(new SbnRequestBeginNotification(sbnContextReference.SbnContext));
                }
                catch (Exception ex)
                {
                    // try catch so we don't kill everything in all requests
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    try
                    {
                        await next(context);

                    }
                    finally
                    {
                        await _eventAggregator.PublishAsync(new SbnRequestEndNotification(sbnContextReference.SbnContext));
                    }
                }
            }
            finally
            {
                // Verbose log end of every request (in v8 we didn't log the end request of ALL requests, only the front-end which was
                // strange since we always logged the beginning, so now we just log start/end of all requests)
                LogHttpRequest.TryGetCurrentHttpRequestId(out Guid httpRequestId, _requestCache);
                _logger.LogTrace("End Request [{HttpRequestId}]: {RequestUrl} ({RequestDuration}ms)", httpRequestId, pathAndQuery, DateTime.Now.Subtract(sbnContextReference.SbnContext.ObjectCreated).TotalMilliseconds);

                try
                {
                    DisposeHttpContextItems(context.Request);
                }
                finally
                {
                    // Dispose the sbn context reference which will in turn dispose the SbnContext itself.
                    sbnContextReference.Dispose();
                }
            }

            // Profiling end needs to be last of the first things that happens.
            // Also MiniProfiler.Current becomes null if it is handled by the event aggregator due to async/await
            _profiler?.SbnApplicationEndRequest(context, _runtimeState.Level);
        }

        /// <summary>
        /// Used to lazily initialize any back office services when the first request to the back office is made
        /// </summary>
        /// <param name="sbnContext"></param>
        /// <returns></returns>
        private void LazyInitializeBackOfficeServices(PathString absPath)
        {
            if (s_firstBackOfficeRequest)
            {
                return;
            }

            if (_sbnRequestPaths.IsBackOfficeRequest(absPath)
                || absPath.Value.InvariantStartsWith($"/{_smidgeOptions.UrlOptions.CompositeFilePath}")
                || absPath.Value.InvariantStartsWith($"/{_smidgeOptions.UrlOptions.BundleFilePath}"))
            {
                LazyInitializer.EnsureInitialized(ref s_firstBackOfficeRequest, ref s_firstBackOfficeReqestFlag, ref s_firstBackOfficeRequestLocker, () =>
                {
                    _backOfficeWebAssets.CreateBundles();
                    return true;
                });
            }
        }

        private Uri GetApplicationUrlFromCurrentRequest(HttpRequest request)
        {
            // We only consider GET and POST.
            // Especially the DEBUG sent when debugging the application is annoying because it uses http, even when the https is available.
            if (request.Method == "GET" || request.Method == "POST")
            {
                return new Uri($"{request.Scheme}://{request.Host}{request.PathBase}", UriKind.Absolute);

            }
            return null;
        }

        /// <summary>
        /// Dispose some request scoped objects that we are maintaining the lifecycle for.
        /// </summary>
        private void DisposeHttpContextItems(HttpRequest request)
        {
            // do not process if client-side request
            if (request.IsClientSideRequest())
            {
                return;
            }

            // ensure this is disposed by DI at the end of the request
            IHttpScopeReference httpScopeReference = request.HttpContext.RequestServices.GetRequiredService<IHttpScopeReference>();
            httpScopeReference.Register();
        }

        /// <summary>
        /// Initializes the content cache one time
        /// </summary>
        private void EnsureContentCacheInitialized() => LazyInitializer.EnsureInitialized(
            ref s_cacheInitialized,
            ref s_cacheInitializedFlag,
            ref s_cacheInitializedLock,
            () =>
            {
                _publishedSnapshotServiceEventHandler.Initialize();
                return true;
            });
    }
}
