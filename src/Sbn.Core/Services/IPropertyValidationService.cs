using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PropertyEditors;

namespace Sbn.Cms.Core.Services
{
    public interface IPropertyValidationService
    {
        /// <summary>
        /// Validates the content item's properties pass validation rules
        /// </summary>
        bool IsPropertyDataValid(IContent content, out IProperty[] invalidProperties, CultureImpact impact);

        /// <summary>
        /// Gets a value indicating whether the property has valid values.
        /// </summary>
        bool IsPropertyValid(IProperty property, string culture = "*", string segment = "*");

        /// <summary>
        /// Validates a property value.
        /// </summary>
        IEnumerable<ValidationResult> ValidatePropertyValue(
            IDataEditor editor,
            IDataType dataType,
            object postedValue,
            bool isRequired,
            string validationRegExp,
            string isRequiredMessage,
            string validationRegExpMessage);

        /// <summary>
        /// Validates a property value.
        /// </summary>
        IEnumerable<ValidationResult> ValidatePropertyValue(
            IPropertyType propertyType,
            object postedValue);
    }
}
