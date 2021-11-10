using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.ContentApps
{
    public class ContentTypeDesignContentAppFactory : IContentAppFactory
    {
        private const int Weight = -200;

        private ContentApp _contentTypeApp;

        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            switch (source)
            {
                case IContentType _:
                    return _contentTypeApp ?? (_contentTypeApp = new ContentApp()
                    {
                        Alias = "design",
                        Name = "Design",
                        Icon = "icon-document-dashed-line",
                        View = "views/documentTypes/views/design/design.html",
                        Weight = Weight
                    });
                default:
                    return null;
            }
        }
    }
}
