using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.Mapping;
using Sbn.Cms.Core.Security;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Registers the core Sbn mapper definitions
        /// </summary>
        public static ISbnBuilder AddCoreMappingProfiles(this ISbnBuilder builder)
        {
            builder.Services.AddUnique<ISbnMapper, SbnMapper>();

            builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<AuditMapDefinition>()
                .Add<CodeFileMapDefinition>()
                .Add<ContentPropertyMapDefinition>()
                .Add<ContentTypeMapDefinition>()
                .Add<DataTypeMapDefinition>()
                .Add<EntityMapDefinition>()
                .Add<DictionaryMapDefinition>()
                .Add<MacroMapDefinition>()
                .Add<RedirectUrlMapDefinition>()
                .Add<RelationMapDefinition>()
                .Add<SectionMapDefinition>()
                .Add<TagMapDefinition>()
                .Add<TemplateMapDefinition>()
                .Add<UserMapDefinition>()
                .Add<MemberMapDefinition>()
                .Add<LanguageMapDefinition>()
                .Add<IdentityMapDefinition>();

            builder.Services.AddTransient<CommonMapper>();
            builder.Services.AddTransient<MemberTabsAndPropertiesMapper>();

            return builder;
        }
    }
}
