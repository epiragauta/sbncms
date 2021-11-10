using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Services
{
    public interface IIdKeyMap
    {
        Attempt<int> GetIdForKey(Guid key, SbnObjectTypes sbnObjectType);
        Attempt<int> GetIdForUdi(Udi udi);
        Attempt<Udi> GetUdiForId(int id, SbnObjectTypes sbnObjectType);
        Attempt<Guid> GetKeyForId(int id, SbnObjectTypes sbnObjectType);
        void ClearCache();
        void ClearCache(int id);
        void ClearCache(Guid key);
    }
}
