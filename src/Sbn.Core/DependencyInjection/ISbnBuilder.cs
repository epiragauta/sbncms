using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Logging;

namespace Sbn.Cms.Core.DependencyInjection
{
    public interface ISbnBuilder
    {
        IServiceCollection Services { get; }
        IConfiguration Config { get; }
        TypeLoader TypeLoader { get; }

        /// <summary>
        /// A Logger factory created specifically for the <see cref="ISbnBuilder"/>. This is NOT the same
        /// instance that will be resolved from DI. Use only if required during configuration.
        /// </summary>
        ILoggerFactory BuilderLoggerFactory { get; }

        /// <summary>
        /// A hosting environment created specifically for the <see cref="ISbnBuilder"/>. This is NOT the same
        /// instance that will be resolved from DI. Use only if required during configuration.
        /// </summary>
        /// <remarks>
        /// This may be null.
        /// </remarks>
        IHostingEnvironment BuilderHostingEnvironment { get; }

        IProfiler Profiler { get; }
        AppCaches AppCaches { get; }
        TBuilder WithCollectionBuilder<TBuilder>() where TBuilder : ICollectionBuilder, new();
        void Build();
    }
}
