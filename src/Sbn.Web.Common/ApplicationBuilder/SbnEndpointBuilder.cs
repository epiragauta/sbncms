using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    /// <summary>
    /// A builder to allow encapsulating the enabled endpoints in Sbn
    /// </summary>
    internal class SbnEndpointBuilder : ISbnEndpointBuilderContext
    {
        public SbnEndpointBuilder(IServiceProvider services, IRuntimeState runtimeState, IApplicationBuilder appBuilder, IEndpointRouteBuilder endpointRouteBuilder)
        {
            ApplicationServices = services;
            EndpointRouteBuilder = endpointRouteBuilder;
            RuntimeState = runtimeState;
            AppBuilder = appBuilder;
        }

        public IServiceProvider ApplicationServices { get; }
        public IEndpointRouteBuilder EndpointRouteBuilder { get; }
        public IRuntimeState RuntimeState { get; }
        public IApplicationBuilder AppBuilder { get; }
    }
}
