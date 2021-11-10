// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core;
using Sbn.Cms.Core.Models;

namespace Sbn.Extensions
{
    public static class RelationTypeExtensions
    {
        public static bool IsSystemRelationType(this IRelationType relationType) =>
            relationType.Alias == Constants.Conventions.RelationTypes.RelatedDocumentAlias
            || relationType.Alias == Constants.Conventions.RelationTypes.RelatedMediaAlias
            || relationType.Alias == Constants.Conventions.RelationTypes.RelateDocumentOnCopyAlias
            || relationType.Alias == Constants.Conventions.RelationTypes.RelateParentDocumentOnDeleteAlias
            || relationType.Alias == Constants.Conventions.RelationTypes.RelateParentMediaFolderOnDeleteAlias;
    }
}
