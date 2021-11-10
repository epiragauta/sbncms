using Examine;

namespace Sbn.Cms.Infrastructure.Examine
{
    public interface ISbnIndexConfig
    {
        IContentValueSetValidator GetContentValueSetValidator();
        IContentValueSetValidator GetPublishedContentValueSetValidator();
        IValueSetValidator GetMemberValueSetValidator();

    }
}
