using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Infrastructure.DependencyInjection;
using Sbn.Cms.Web.Common.Middleware;
using Sbn.Cms.Web.Common.Routing;
using Sbn.Cms.Web.Website.Collections;
using Sbn.Cms.Web.Website.Models;
using Sbn.Cms.Web.Website.Routing;
using Sbn.Cms.Web.Website.ViewEngines;
using static Microsoft.Extensions.DependencyInjection.ServiceDescriptor;

namespace Sbn.Extensions
{
    /// <summary>
    /// <see cref="ISbnBuilder"/> extensions for sbn front-end website
    /// </summary>
    public static class SbnBuilderExtensions
    {
        /// <summary>
        /// Add services for the sbn front-end website
        /// </summary>
        public static ISbnBuilder AddWebsite(this ISbnBuilder builder)
        {
            builder.WithCollectionBuilder<SurfaceControllerTypeCollectionBuilder>()
                 .Add(builder.TypeLoader.GetSurfaceControllers());

            // Configure MVC startup options for custom view locations
            builder.Services.ConfigureOptions<RenderRazorViewEngineOptionsSetup>();
            builder.Services.ConfigureOptions<PluginRazorViewEngineOptionsSetup>();

            // Wraps all existing view engines in a ProfilerViewEngine
            builder.Services.AddTransient<IConfigureOptions<MvcViewOptions>, ProfilingViewEngineWrapperMvcViewOptionsSetup>();

            // TODO figure out if we need more to work on load balanced setups
            builder.Services.AddDataProtection();
            builder.Services.AddAntiforgery();

            builder.Services.AddSingleton<SbnRouteValueTransformer>();
            builder.Services.AddSingleton<IControllerActionSearcher, ControllerActionSearcher>();
            builder.Services.TryAddEnumerable(Singleton<MatcherPolicy, NotFoundSelectorPolicy>());
            builder.Services.AddSingleton<ISbnRouteValuesFactory, SbnRouteValuesFactory>();
            builder.Services.AddSingleton<IRoutableDocumentFilter, RoutableDocumentFilter>();

            builder.Services.AddSingleton<FrontEndRoutes>();

            builder.Services.AddSingleton<MemberModelBuilderFactory>();

            builder.Services.AddSingleton<IPublicAccessRequestHandler, PublicAccessRequestHandler>();
            builder.Services.AddSingleton<BasicAuthenticationMiddleware>();

            builder
                .AddDistributedCache()
                .AddModelsBuilder();

            builder.AddMembersIdentity();

            return builder;
        }
    }
}
