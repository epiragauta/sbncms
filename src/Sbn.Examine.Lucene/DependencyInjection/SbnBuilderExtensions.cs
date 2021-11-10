using Examine;
using Examine.Lucene.Directories;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Infrastructure.DependencyInjection;

namespace Sbn.Cms.Infrastructure.Examine.DependencyInjection
{
    public static class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds the Examine indexes for Sbn
        /// </summary>
        /// <param name="sbnBuilder"></param>
        /// <returns></returns>
        public static ISbnBuilder AddExamineIndexes(this ISbnBuilder sbnBuilder)
        {
            IServiceCollection services = sbnBuilder.Services;

            services.AddSingleton<IBackOfficeExamineSearcher, BackOfficeExamineSearcher>();
            services.AddSingleton<IIndexDiagnosticsFactory, LuceneIndexDiagnosticsFactory>();

            services.AddExamine();

            // Create the indexes
            services
                .AddExamineLuceneIndex<SbnContentIndex, ConfigurationEnabledDirectoryFactory>(Constants.SbnIndexes.InternalIndexName)
                .AddExamineLuceneIndex<SbnContentIndex, ConfigurationEnabledDirectoryFactory>(Constants.SbnIndexes.ExternalIndexName)
                .AddExamineLuceneIndex<SbnMemberIndex, ConfigurationEnabledDirectoryFactory>(Constants.SbnIndexes.MembersIndexName)
                .ConfigureOptions<ConfigureIndexOptions>();

            services.AddSingleton<IApplicationRoot, SbnApplicationRoot>();
            services.AddSingleton<ILockFactory, SbnLockFactory>();
            services.AddSingleton<ConfigurationEnabledDirectoryFactory>();

            return sbnBuilder;
        }
    }
}
