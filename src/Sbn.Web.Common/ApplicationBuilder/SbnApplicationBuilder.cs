using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web.DependencyInjection;
using Sbn.Cms.Core.Services;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    /// <summary>
    /// A builder used to enable middleware and endpoints required for Sbn to operate.
    /// </summary>
    /// <remarks>
    /// This helps to ensure that everything is registered in the correct order.
    /// </remarks>
    public class SbnApplicationBuilder : ISbnApplicationBuilder, ISbnEndpointBuilder, ISbnApplicationBuilderContext
    {
        private readonly IOptions<SbnPipelineOptions> _sbnPipelineStartupOptions;

        public SbnApplicationBuilder(IApplicationBuilder appBuilder)
        {
            AppBuilder = appBuilder ?? throw new ArgumentNullException(nameof(appBuilder));
            ApplicationServices = appBuilder.ApplicationServices;
            RuntimeState = appBuilder.ApplicationServices.GetRequiredService<IRuntimeState>();            
            _sbnPipelineStartupOptions = ApplicationServices.GetRequiredService<IOptions<SbnPipelineOptions>>();
        }

        public IServiceProvider ApplicationServices { get; }
        public IRuntimeState RuntimeState { get; }
        public IApplicationBuilder AppBuilder { get; }

        /// <inheritdoc />
        public ISbnEndpointBuilder WithCustomMiddleware(Action<ISbnApplicationBuilderContext> configureSbnMiddleware)
        {
            if (configureSbnMiddleware is null)
            {
                throw new ArgumentNullException(nameof(configureSbnMiddleware));
            }

            configureSbnMiddleware(this);

            return this;
        }

        /// <inheritdoc />
        public ISbnEndpointBuilder WithMiddleware(Action<ISbnApplicationBuilderContext> configureSbnMiddleware)
        {
            if (configureSbnMiddleware is null)
            {
                throw new ArgumentNullException(nameof(configureSbnMiddleware));
            }

            RunPrePipeline();

            RegisterDefaultRequiredMiddleware();

            RunPostPipeline();

            configureSbnMiddleware(this);

            return this;
        }

        /// <inheritdoc />
        public void WithEndpoints(Action<ISbnEndpointBuilderContext> configureSbn)
        {
            IOptions<SbnPipelineOptions> startupOptions = ApplicationServices.GetRequiredService<IOptions<SbnPipelineOptions>>();
            RunPreEndpointsPipeline();

            AppBuilder.UseEndpoints(endpoints =>
            {
                var umbAppBuilder = (ISbnEndpointBuilderContext)ActivatorUtilities.CreateInstance<SbnEndpointBuilder>(
                    ApplicationServices,
                    new object[] { AppBuilder, endpoints });
                configureSbn(umbAppBuilder);
            });
        }

        /// <summary>
        /// Registers the default required middleware to run Sbn
        /// </summary>
        /// <param name="sbnApplicationBuilderContext"></param>
        public void RegisterDefaultRequiredMiddleware()
        {
            UseSbnCoreMiddleware();

            AppBuilder.UseStatusCodePages();

            // Important we handle image manipulations before the static files, otherwise the querystring is just ignored.            
            AppBuilder.UseImageSharp();
            AppBuilder.UseStaticFiles();
            AppBuilder.UseSbnPluginsStaticFiles();

            // UseRouting adds endpoint routing middleware, this means that middlewares registered after this one
            // will execute after endpoint routing. The ordering of everything is quite important here, see
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-5.0
            // where we need to have UseAuthentication and UseAuthorization proceeding this call but before
            // endpoints are defined.
            AppBuilder.UseRouting();
            AppBuilder.UseAuthentication();
            AppBuilder.UseAuthorization();

            // This must come after auth because the culture is based on the auth'd user
            AppBuilder.UseRequestLocalization();

            // Must be called after UseRouting and before UseEndpoints
            AppBuilder.UseSession();

            // DO NOT PUT ANY UseEndpoints declarations here!! Those must all come very last in the pipeline,
            // endpoints are terminating middleware. All of our endpoints are declared in ext of ISbnApplicationBuilder
        }

        public void UseSbnCoreMiddleware()
        {
            AppBuilder.UseSbnCore();
            AppBuilder.UseSbnRequestLogging();

            // We need to add this before UseRouting so that the SbnContext and other middlewares are executed
            // before endpoint routing middleware.
            AppBuilder.UseSbnRouting();
        }

        public void RunPrePipeline()
        {
            foreach (ISbnPipelineFilter filter in _sbnPipelineStartupOptions.Value.PipelineFilters)
            {
                filter.OnPrePipeline(AppBuilder);
            }
        }

        public void RunPostPipeline()
        {
            foreach (ISbnPipelineFilter filter in _sbnPipelineStartupOptions.Value.PipelineFilters)
            {
                filter.OnPostPipeline(AppBuilder);
            }
        }

        private void RunPreEndpointsPipeline()
        {
            foreach (ISbnPipelineFilter filter in _sbnPipelineStartupOptions.Value.PipelineFilters)
            {
                filter.OnEndpoints(AppBuilder);
            }
        }
    }
}
