using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.ContentEditing;

namespace Sbn.Cms.Core.Models.Mapping
{
    public class CodeFileMapDefinition : IMapDefinition
    {
        public void DefineMaps(ISbnMapper mapper)
        {
            mapper.Define<IStylesheet, EntityBasic>((source, context) => new EntityBasic(), Map);
            mapper.Define<IStylesheet, CodeFileDisplay>((source, context) => new CodeFileDisplay(), Map);

            mapper.Define<IPartialView, EntityBasic>((source, context) => new EntityBasic(), Map);
            mapper.Define<IPartialView, CodeFileDisplay>((source, context) => new CodeFileDisplay(), Map);

            mapper.Define<IScript, EntityBasic>((source, context) => new EntityBasic(), Map);
            mapper.Define<IScript, CodeFileDisplay>((source, context) => new CodeFileDisplay(), Map);

            mapper.Define<CodeFileDisplay, IPartialView>(Map);
            mapper.Define<CodeFileDisplay, IScript>(Map);

        }

        // Sbn.Code.MapAll -Trashed -Udi -Icon
        private static void Map(IStylesheet source, EntityBasic target, MapperContext context)
        {
            target.Alias = source.Alias;
            target.Id = source.Id;
            target.Key = source.Key;
            target.Name = source.Name;
            target.ParentId = -1;
            target.Path = source.Path;
        }

        // Sbn.Code.MapAll -Trashed -Udi -Icon
        private static void Map(IScript source, EntityBasic target, MapperContext context)
        {
            target.Alias = source.Alias;
            target.Id = source.Id;
            target.Key = source.Key;
            target.Name = source.Name;
            target.ParentId = -1;
            target.Path = source.Path;
        }

        // Sbn.Code.MapAll -Trashed -Udi -Icon
        private static void Map(IPartialView source, EntityBasic target, MapperContext context)
        {
            target.Alias = source.Alias;
            target.Id = source.Id;
            target.Key = source.Key;
            target.Name = source.Name;
            target.ParentId = -1;
            target.Path = source.Path;
        }

        // Sbn.Code.MapAll -FileType -Notifications -Path -Snippet
        private static void Map(IPartialView source, CodeFileDisplay target, MapperContext context)
        {
            target.Content = source.Content;
            target.Id = source.Id.ToString();
            target.Name = source.Name;
            target.VirtualPath = source.VirtualPath;
        }

        // Sbn.Code.MapAll -FileType -Notifications -Path -Snippet
        private static void Map(IScript source, CodeFileDisplay target, MapperContext context)
        {
            target.Content = source.Content;
            target.Id = source.Id.ToString();
            target.Name = source.Name;
            target.VirtualPath = source.VirtualPath;
        }

        // Sbn.Code.MapAll -FileType -Notifications -Path -Snippet
        private static void Map(IStylesheet source, CodeFileDisplay target, MapperContext context)
        {
            target.Content = source.Content;
            target.Id = source.Id.ToString();
            target.Name = source.Name;
            target.VirtualPath = source.VirtualPath;
        }

        // Sbn.Code.MapAll -CreateDate -DeleteDate -UpdateDate
        // Sbn.Code.MapAll -Id -Key -Alias -Name -OriginalPath -Path
        private static void Map(CodeFileDisplay source, IPartialView target, MapperContext context)
        {
            target.Content = source.Content;
            target.VirtualPath = source.VirtualPath;
        }

        // Sbn.Code.MapAll -CreateDate -DeleteDate -UpdateDate -GetFileContent
        // Sbn.Code.MapAll -Id -Key -Alias -Name -OriginalPath -Path
        private static void Map(CodeFileDisplay source, IScript target, MapperContext context)
        {
            target.Content = source.Content;
            target.VirtualPath = source.VirtualPath;
        }
    }
}
