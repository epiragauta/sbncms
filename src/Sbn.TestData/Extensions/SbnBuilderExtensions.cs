using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Infrastructure.PublishedCache;
using Sbn.Cms.Web.Common.ApplicationBuilder;
using Sbn.TestData.Configuration;

namespace Sbn.TestData.Extensions
{
    public static class SbnBuilderExtensions
    {
        public static ISbnBuilder AddSbnTestData(this ISbnBuilder builder)
        {
            if (builder.Services.Any(x => x.ServiceType == typeof(LoadTestController)))
            {
                // We assume the test data project is composed if any implementations of LoadTestController exist.
                return builder;
            }

            IConfigurationSection testDataSection = builder.Config.GetSection("Sbn:CMS:TestData");
            TestDataSettings config = testDataSection.Get<TestDataSettings>();
            if (config == null || config.Enabled == false)
            {
                return builder;
            }

            builder.Services.Configure<TestDataSettings>(testDataSection);

            if (config.IgnoreLocalDb)
            {
                builder.Services.AddSingleton(factory => new PublishedSnapshotServiceOptions
                {
                    IgnoreLocalDb = true
                });
            }

            builder.Services.Configure<SbnPipelineOptions>(options =>
                options.AddFilter(new SbnPipelineFilter(nameof(LoadTestController))
                {
                    Endpoints = app => app.UseEndpoints(endpoints =>
                        endpoints.MapControllerRoute(
                            "LoadTest",
                            "/LoadTest/{action}",
                            new { controller = "LoadTest", Action = "Index" }))
                }));

            builder.Services.AddScoped(typeof(LoadTestController));

            return builder;
        }
    }
}
