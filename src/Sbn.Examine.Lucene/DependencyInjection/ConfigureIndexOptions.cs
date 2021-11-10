using System;
using Examine;
using Examine.Lucene;
using Examine.Lucene.Analyzers;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;

namespace Sbn.Cms.Infrastructure.Examine.DependencyInjection
{
    /// <summary>
    /// Configures the index options to construct the Examine indexes
    /// </summary>
    public sealed class ConfigureIndexOptions : IConfigureNamedOptions<LuceneDirectoryIndexOptions>
    {
        private readonly ISbnIndexConfig _sbnIndexConfig;
        private readonly IOptions<IndexCreatorSettings> _settings;

        public ConfigureIndexOptions(
            ISbnIndexConfig sbnIndexConfig,
            IOptions<IndexCreatorSettings> settings)
        {
            _sbnIndexConfig = sbnIndexConfig;
            _settings = settings;
        }

        public void Configure(string name, LuceneDirectoryIndexOptions options)
        {
            switch (name)
            {
                case Constants.SbnIndexes.InternalIndexName:
                    options.Analyzer = new CultureInvariantWhitespaceAnalyzer();
                    options.Validator = _sbnIndexConfig.GetContentValueSetValidator();
                    options.FieldDefinitions = new SbnFieldDefinitionCollection();
                    break;
                case Constants.SbnIndexes.ExternalIndexName:
                    options.Analyzer = new StandardAnalyzer(LuceneInfo.CurrentVersion);
                    options.Validator = _sbnIndexConfig.GetPublishedContentValueSetValidator();
                    options.FieldDefinitions = new SbnFieldDefinitionCollection();
                    break;
                case Constants.SbnIndexes.MembersIndexName:
                    options.Analyzer = new CultureInvariantWhitespaceAnalyzer();
                    options.Validator = _sbnIndexConfig.GetMemberValueSetValidator();
                    options.FieldDefinitions = new SbnFieldDefinitionCollection();
                    break;
            }

            // ensure indexes are unlocked on startup
            options.UnlockIndex = true;

            if (_settings.Value.LuceneDirectoryFactory == LuceneDirectoryFactory.SyncedTempFileSystemDirectoryFactory)
            {
                // if this directory factory is enabled then a snapshot deletion policy is required
                options.IndexDeletionPolicy = new SnapshotDeletionPolicy(new KeepOnlyLastCommitDeletionPolicy());
            }
            
            
        }

        public void Configure(LuceneDirectoryIndexOptions options)
            => throw new NotImplementedException("This is never called and is just part of the interface");
    }
}
