using System;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Extensions;

namespace Sbn.Cms.Core.PropertyEditors.ValueConverters
{
    [DefaultPropertyValueConverter]
    public class MemberPickerValueConverter : PropertyValueConverterBase
    {
        private readonly IMemberService _memberService;
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
        private readonly ISbnContextAccessor _sbnContextAccessor;

        public MemberPickerValueConverter(
            IMemberService memberService,
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            ISbnContextAccessor sbnContextAccessor)
        {
            _memberService = memberService;
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
            _sbnContextAccessor = sbnContextAccessor;
        }

        public override bool IsConverter(IPublishedPropertyType propertyType)
            => propertyType.EditorAlias.InvariantEquals(Constants.PropertyEditors.Aliases.MemberPicker);

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
            => PropertyCacheLevel.Snapshot;

        public override Type GetPropertyValueType(IPublishedPropertyType propertyType)
            => typeof(IPublishedContent);

        public override object ConvertSourceToIntermediate(IPublishedElement owner, IPublishedPropertyType propertyType, object source, bool preview)
        {
            if (source == null)
                return null;

            var attemptConvertInt = source.TryConvertTo<int>();
            if (attemptConvertInt.Success)
                return attemptConvertInt.Result;
            var attemptConvertUdi = source.TryConvertTo<Udi>();
            if (attemptConvertUdi.Success)
                return attemptConvertUdi.Result;
            return null;
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel cacheLevel, object source, bool preview)
        {
            if (source == null)
            {
                return null;
            }

            IPublishedContent member;
            var publishedSnapshot = _publishedSnapshotAccessor.GetRequiredPublishedSnapshot();
            if (source is int id)
            {
                IMember m = _memberService.GetById(id);
                if (m == null)
                {
                    return null;
                }
                member = publishedSnapshot.Members.Get(m);
                if (member != null)
                {
                    return member;
                }
            }
            else
            {
                var sourceUdi = source as GuidUdi;
                if (sourceUdi == null)
                    return null;

                IMember m = _memberService.GetByKey(sourceUdi.Guid);
                if (m == null)
                {
                    return null;
                }

                member = publishedSnapshot.Members.Get(m);

                if (member != null)
                {
                    return member;
                }
            }

            return source;
        }
    }
}
