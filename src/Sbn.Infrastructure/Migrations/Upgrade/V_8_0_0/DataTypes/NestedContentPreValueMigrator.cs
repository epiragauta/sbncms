using System.Globalization;
using Newtonsoft.Json;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0.DataTypes
{
    class NestedContentPreValueMigrator : DefaultPreValueMigrator //PreValueMigratorBase
    {
        public override bool CanMigrate(string editorAlias)
            => editorAlias == "Sbn.NestedContent";

        // you wish - but NestedContentConfiguration lives in Sbn.Web
        /*
        public override object GetConfiguration(int dataTypeId, string editorAlias, Dictionary<string, PreValueDto> preValues)
        {
            return new NestedContentConfiguration { ... };
        }
        */

        protected override object GetPreValueValue(PreValueDto preValue)
        {
            if (preValue.Alias == "confirmDeletes" ||
                preValue.Alias == "showIcons" ||
                preValue.Alias == "hideLabel")
                return preValue.Value == "1";

            if (preValue.Alias == "minItems" ||
                preValue.Alias == "maxItems")
                return int.TryParse(preValue.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i) ? (int?)i : null;

            return preValue.Value.DetectIsJson() ? JsonConvert.DeserializeObject(preValue.Value) : preValue.Value;
        }
    }
}
