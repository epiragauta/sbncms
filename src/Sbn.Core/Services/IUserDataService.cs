using System.Collections.Generic;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Services
{
    public interface IUserDataService
    {
        IEnumerable<UserData> GetUserData();
    }
}
