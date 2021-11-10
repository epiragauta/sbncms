using System.Threading.Tasks;

namespace Sbn.Cms.Core.Security
{
    public interface IPublicAccessChecker
    {
        Task<PublicAccessStatus> HasMemberAccessToContentAsync(int publishedContentId);
    }
}
