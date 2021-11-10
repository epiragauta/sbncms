using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Infrastructure.WebAssets;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    public static partial class SbnBuilderExtensions
    {
        internal static ISbnBuilder AddWebAssets(this ISbnBuilder builder)
        {
            builder.Services.AddSingleton<BackOfficeWebAssets>();
            return builder;
        }
    }
}
