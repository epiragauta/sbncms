using System.IO;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IFileRepository
    {
        Stream GetFileContentStream(string filepath);

        void SetFileContent(string filepath, Stream content);

        long GetFileSize(string filepath);
    }
}
