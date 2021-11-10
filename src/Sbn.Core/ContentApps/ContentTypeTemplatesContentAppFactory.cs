using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.ContentApps
{
    public class ContentTypeTemplatesContentAppFactory : IContentAppFactory
    {
        private const int Weight = -140;

        private ContentApp _contentTypeApp;

        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            switch (source)
            {
                case IContentType _:
                    return _contentTypeApp ?? (_contentTypeApp = new ContentApp()
                    {
                        Alias = "templates",
                        Name = "Templates",
                        Icon = "icon-layout",
                        View = "views/documentTypes/views/templates/templates.html",
                        Weight = Weight
                    });
                default:
                    return null;
            }
        }
    }
}
