using System;
using Microsoft.AspNetCore.Builder;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Web.Common.ApplicationBuilder
{
    /// <summary>
    /// Services used during the Sbn building phase.
    /// </summary>
    public interface ISbnApplicationBuilderServices
    {
        IApplicationBuilder AppBuilder { get; }
        IServiceProvider ApplicationServices { get; }
        IRuntimeState RuntimeState { get; }
    }
}
