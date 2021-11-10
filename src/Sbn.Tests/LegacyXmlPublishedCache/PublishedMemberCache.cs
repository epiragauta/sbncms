using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Extensions;
using Sbn.Web.Composing;

namespace Sbn.Tests.LegacyXmlPublishedCache
{
    class PublishedMemberCache : IPublishedMemberCache
    {
        private readonly PublishedContentTypeCache _contentTypeCache;
        private readonly IVariationContextAccessor _variationContextAccessor;

        public PublishedMemberCache(PublishedContentTypeCache contentTypeCache, IVariationContextAccessor variationContextAccessor)
        {
            _contentTypeCache = contentTypeCache;
            _variationContextAccessor = variationContextAccessor;
        }

        public IPublishedContent Get(IMember member)
        {
            var type = _contentTypeCache.Get(PublishedItemType.Member, member.ContentTypeId);
            return new PublishedMember(member, type, _variationContextAccessor)
                .CreateModel(Current.PublishedModelFactory);
        }

        #region Content types

        public IPublishedContentType GetContentType(int id) => _contentTypeCache.Get(PublishedItemType.Member, id);

        public IPublishedContentType GetContentType(string alias) => _contentTypeCache.Get(PublishedItemType.Member, alias);

        #endregion
    }
}
