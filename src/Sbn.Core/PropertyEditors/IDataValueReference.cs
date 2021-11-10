using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Editors;

namespace Sbn.Cms.Core.PropertyEditors
{
    /// <summary>
    /// Resolve references from <see cref="IDataValueEditor"/> values
    /// </summary>
    public interface IDataValueReference
    {
        /// <summary>
        /// Returns any references contained in the value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        IEnumerable<SbnEntityReference> GetReferences(object value);
    }
}
