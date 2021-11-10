using System.Collections.Generic;

namespace Sbn.Cms.Core.Models
{
    public interface IStylesheet : IFile
    {
        /// <summary>
        /// Returns a list of sbn back office enabled stylesheet properties
        /// </summary>
        /// <remarks>
        /// An sbn back office enabled stylesheet property has a special prefix, for example:
        ///
        /// /** umb_name: MyPropertyName */ p { font-size: 1em; }
        /// </remarks>
        IEnumerable<IStylesheetProperty> Properties { get; }

        /// <summary>
        /// Adds an Sbn stylesheet property for use in the back office
        /// </summary>
        /// <param name="property"></param>
        void AddProperty(IStylesheetProperty property);

        /// <summary>
        /// Removes an Sbn stylesheet property
        /// </summary>
        /// <param name="name"></param>
        void RemoveProperty(string name);
    }
}
