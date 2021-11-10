using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.PropertyEditors;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0.DataTypes
{
    class ValueListPreValueMigrator : IPreValueMigrator
    {
        private readonly string[] _editors =
        {
            "Sbn.RadioButtonList",
            "Sbn.CheckBoxList",
            "Sbn.DropDown",
            "Sbn.DropdownlistPublishingKeys",
            "Sbn.DropDownMultiple",
            "Sbn.DropdownlistMultiplePublishKeys"
        };

        public bool CanMigrate(string editorAlias)
            => _editors.Contains(editorAlias);

        public virtual string GetNewAlias(string editorAlias)
            => null;

        public object GetConfiguration(int dataTypeId, string editorAlias, Dictionary<string, PreValueDto> preValues)
        {
            var config = new ValueListConfiguration();
            foreach (var preValue in preValues.Values)
                config.Items.Add(new ValueListConfiguration.ValueListItem { Id = preValue.Id, Value = preValue.Value });
            return config;
        }
    }
}
