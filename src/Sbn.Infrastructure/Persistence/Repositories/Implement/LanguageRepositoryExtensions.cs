using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.Persistence.Repositories.Implement
{
    internal static class LanguageRepositoryExtensions
    {
        public static bool IsDefault(this ILanguageRepository repo, string culture)
        {
            if (culture == null || culture == "*") return false;
            return repo.GetDefaultIsoCode().InvariantEquals(culture);
        }
    }
}
