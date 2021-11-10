namespace Sbn.Cms.Core
{
    public static partial class Constants
    {
        public static class SqlTemplates
        {
            public static class VersionableRepository
            {
                public const string GetVersionIds = "Sbn.Core.VersionableRepository.GetVersionIds";
                public const string GetVersion = "Sbn.Core.VersionableRepository.GetVersion";
                public const string GetVersions = "Sbn.Core.VersionableRepository.GetVersions";
                public const string EnsureUniqueNodeName = "Sbn.Core.VersionableRepository.EnsureUniqueNodeName";
                public const string GetSortOrder = "Sbn.Core.VersionableRepository.GetSortOrder";
                public const string GetParentNode = "Sbn.Core.VersionableRepository.GetParentNode";
                public const string GetReservedId = "Sbn.Core.VersionableRepository.GetReservedId";
            }
            public static class RelationRepository
            {
                public const string DeleteByParentAll = "Sbn.Core.RelationRepository.DeleteByParent";
                public const string DeleteByParentIn = "Sbn.Core.RelationRepository.DeleteByParentIn";
            }

            public static class DataTypeRepository
            {
                public const string EnsureUniqueNodeName = "Sbn.Core.DataTypeDefinitionRepository.EnsureUniqueNodeName";
            }

            public static class NuCacheDatabaseDataSource
            {
                public const string WhereNodeId = "Sbn.Web.PublishedCache.NuCache.DataSource.WhereNodeId";
                public const string WhereNodeIdX = "Sbn.Web.PublishedCache.NuCache.DataSource.WhereNodeIdX";
                public const string SourcesSelectSbnNodeJoin = "Sbn.Web.PublishedCache.NuCache.DataSource.SourcesSelectSbnNodeJoin";
                public const string ContentSourcesSelect = "Sbn.Web.PublishedCache.NuCache.DataSource.ContentSourcesSelect";
                public const string ContentSourcesCount = "Sbn.Web.PublishedCache.NuCache.DataSource.ContentSourcesCount";
                public const string MediaSourcesSelect = "Sbn.Web.PublishedCache.NuCache.DataSource.MediaSourcesSelect";
                public const string MediaSourcesCount = "Sbn.Web.PublishedCache.NuCache.DataSource.MediaSourcesCount";
                public const string ObjectTypeNotTrashedFilter = "Sbn.Web.PublishedCache.NuCache.DataSource.ObjectTypeNotTrashedFilter";
                public const string OrderByLevelIdSortOrder = "Sbn.Web.PublishedCache.NuCache.DataSource.OrderByLevelIdSortOrder";

            }
        }
    }
}
