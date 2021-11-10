using System.Collections.Generic;

namespace Sbn.Cms.Core.Models.ContentEditing
{

    public interface IContentProperties<T>
        where T : ContentPropertyBasic
    {
        IEnumerable<T> Properties { get; }
    }
}
