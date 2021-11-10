using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    public interface IMediaRepository : IContentRepository<int, IMedia>, IReadRepository<Guid, IMedia>
    {
        IMedia GetMediaByPath(string mediaPath);
        bool RecycleBinSmells();
    }
}
