using Examine;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Core.Services;

namespace Sbn.Cms.Infrastructure.Examine
{
    public class SbnIndexConfig : ISbnIndexConfig
    {

        public SbnIndexConfig(IPublicAccessService publicAccessService, IScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
            PublicAccessService = publicAccessService;
        }

        protected IPublicAccessService PublicAccessService { get; }
        protected IScopeProvider ScopeProvider { get; }
        public IContentValueSetValidator GetContentValueSetValidator()
        {
            return new ContentValueSetValidator(false, true, PublicAccessService, ScopeProvider);
        }

        public IContentValueSetValidator GetPublishedContentValueSetValidator()
        {
            return new ContentValueSetValidator(true, false, PublicAccessService, ScopeProvider);
        }

        /// <summary>
        /// Returns the <see cref="IValueSetValidator"/> for the member indexer
        /// </summary>
        /// <returns></returns>
        public IValueSetValidator GetMemberValueSetValidator()
        {
            return new MemberValueSetValidator();
        }
    }
}
