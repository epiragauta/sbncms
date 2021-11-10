using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Web.BackOffice.Mapping;

namespace Sbn.Extensions
{
    public static class WebMappingProfiles
    {
        public static ISbnBuilder AddWebMappingProfiles(this ISbnBuilder builder)
        {
            builder.WithCollectionBuilder<MapDefinitionCollectionBuilder>()
                .Add<ContentMapDefinition>()
                .Add<MediaMapDefinition>()
                .Add<MemberMapDefinition>();

            builder.Services.AddTransient<CommonTreeNodeMapper>();

            return builder;
        }
    }
}
