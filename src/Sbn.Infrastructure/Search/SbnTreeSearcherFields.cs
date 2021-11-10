
using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Infrastructure.Examine;

namespace Sbn.Cms.Infrastructure.Search
{
    public class SbnTreeSearcherFields : ISbnTreeSearcherFields
    {
        private IReadOnlyList<string> _backOfficeFields = new List<string> {"id", SbnExamineFieldNames.ItemIdFieldName, SbnExamineFieldNames.NodeKeyFieldName};
        private readonly ISet<string> _backOfficeFieldsToLoad = new HashSet<string> { "id", SbnExamineFieldNames.ItemIdFieldName, SbnExamineFieldNames.NodeKeyFieldName, "nodeName", SbnExamineFieldNames.IconFieldName, SbnExamineFieldNames.CategoryFieldName, "parentID", SbnExamineFieldNames.ItemTypeFieldName };
        private IReadOnlyList<string> _backOfficeMediaFields = new List<string> { SbnExamineFieldNames.SbnFileFieldName };
        private readonly ISet<string> _backOfficeMediaFieldsToLoad = new HashSet<string> { SbnExamineFieldNames.SbnFileFieldName };
        private IReadOnlyList<string> _backOfficeMembersFields = new List<string> { "email", "loginName" };
        private readonly ISet<string> _backOfficeMembersFieldsToLoad = new HashSet<string> { "email", "loginName" };
        private readonly ISet<string> _backOfficeDocumentFieldsToLoad = new HashSet<string> { SbnExamineFieldNames.VariesByCultureFieldName };
        private readonly ILocalizationService _localizationService;

        public SbnTreeSearcherFields(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        /// <inheritdoc />
        public IEnumerable<string> GetBackOfficeFields() => _backOfficeFields;

        /// <inheritdoc />
        public IEnumerable<string> GetBackOfficeMembersFields() => _backOfficeMembersFields;

        /// <inheritdoc />
        public IEnumerable<string> GetBackOfficeMediaFields() => _backOfficeMediaFields;

        /// <inheritdoc />
        public IEnumerable<string> GetBackOfficeDocumentFields() => Enumerable.Empty<string>();

        /// <inheritdoc />
        public ISet<string> GetBackOfficeFieldsToLoad() => _backOfficeFieldsToLoad;

        /// <inheritdoc />
        public ISet<string> GetBackOfficeMembersFieldsToLoad() => _backOfficeMembersFieldsToLoad;

        /// <inheritdoc />
        public ISet<string> GetBackOfficeMediaFieldsToLoad() => _backOfficeMediaFieldsToLoad;

        /// <inheritdoc />
        public ISet<string> GetBackOfficeDocumentFieldsToLoad()
        {
            var fields = _backOfficeDocumentFieldsToLoad;

            // We need to load all nodeName_* fields but we won't know those up front so need to get
            // all langs (this is cached)
            foreach(var field in _localizationService.GetAllLanguages().Select(x => "nodeName_" + x.IsoCode.ToLowerInvariant()))
            {
                fields.Add(field);
            }

            return fields;
        }
    }
}
