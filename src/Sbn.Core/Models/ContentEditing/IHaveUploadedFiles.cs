using System.Collections.Generic;
using Sbn.Cms.Core.Models.Editors;

namespace Sbn.Cms.Core.Models.ContentEditing
{
    public interface IHaveUploadedFiles
    {
        List<ContentPropertyFile> UploadedFiles { get; }
    }
}
