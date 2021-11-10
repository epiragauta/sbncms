using System;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Infrastructure.Persistence.Dtos;

namespace Sbn.Cms.Infrastructure.Persistence.Mappers
{
    [MapperFor(typeof(IDomain))]
    [MapperFor(typeof(SbnDomain))]
    public sealed class DomainMapper : BaseMapper
    {
        public DomainMapper(Lazy<ISqlContext> sqlContext, MapperConfigurationStore maps)
            : base(sqlContext, maps)
        { }

        protected override void DefineMaps()
        {
            DefineMap<SbnDomain, DomainDto>(nameof(SbnDomain.Id), nameof(DomainDto.Id));
            DefineMap<SbnDomain, DomainDto>(nameof(SbnDomain.RootContentId), nameof(DomainDto.RootStructureId));
            DefineMap<SbnDomain, DomainDto>(nameof(SbnDomain.LanguageId), nameof(DomainDto.DefaultLanguage));
            DefineMap<SbnDomain, DomainDto>(nameof(SbnDomain.DomainName), nameof(DomainDto.DomainName));
        }
    }
}
