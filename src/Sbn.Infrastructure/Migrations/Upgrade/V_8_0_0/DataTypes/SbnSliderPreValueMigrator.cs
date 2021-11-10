using System.Collections.Generic;
using Sbn.Cms.Core.PropertyEditors;

namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0.DataTypes
{
    class SbnSliderPreValueMigrator : PreValueMigratorBase
    {
        public override bool CanMigrate(string editorAlias)
            => editorAlias == "Sbn.Slider";

        public override object GetConfiguration(int dataTypeId, string editorAlias, Dictionary<string, PreValueDto> preValues)
        {
            return new SliderConfiguration
            {
                EnableRange = GetBoolValue(preValues, "enableRange"),
                InitialValue = GetDecimalValue(preValues, "initVal1"),
                InitialValue2 = GetDecimalValue(preValues, "initVal2"),
                MaximumValue = GetDecimalValue(preValues, "maxVal"),
                MinimumValue = GetDecimalValue(preValues, "minVal"),
                StepIncrements = GetDecimalValue(preValues, "step")
            };
        }
    }
}
