using Sbn.Cms.Core.Models.Entities;

namespace Sbn.Cms.Core.Models
{
    public sealed class Folder : EntityBase
    {
        public Folder(string folderPath)
        {
            Path = folderPath;
        }

        public string Path { get; set; }
    }
}
