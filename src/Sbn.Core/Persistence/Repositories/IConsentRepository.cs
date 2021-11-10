using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.Persistence.Repositories
{
    /// <summary>
    /// Represents a repository for <see cref="IConsent"/> entities.
    /// </summary>
    public interface IConsentRepository : IReadWriteQueryRepository<int, IConsent>
    {
        /// <summary>
        /// Clears the current flag.
        /// </summary>
        void ClearCurrent(string source, string context, string action);
    }
}
