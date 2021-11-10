using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Serialization;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Core.Security
{

    /// <summary>
    /// A password hasher for back office users
    /// </summary>
    /// <remarks>
    /// This allows us to verify passwords in old formats and roll forward to the latest format
    /// </remarks>
    public class BackOfficePasswordHasher : SbnPasswordHasher<BackOfficeIdentityUser>
    {
        public BackOfficePasswordHasher(LegacyPasswordSecurity passwordSecurity, IJsonSerializer jsonSerializer)
            : base(passwordSecurity, jsonSerializer)
        {
        }
    }
}
