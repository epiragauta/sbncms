using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Services
{
    public interface IMemberGroupService : IService
    {
        IEnumerable<IMemberGroup> GetAll();
        IMemberGroup GetById(int id);
        IMemberGroup GetById(Guid id);
        IEnumerable<IMemberGroup> GetByIds(IEnumerable<int> ids);
        IMemberGroup GetByName(string name);
        void Save(IMemberGroup memberGroup);
        void Delete(IMemberGroup memberGroup);
    }
}
