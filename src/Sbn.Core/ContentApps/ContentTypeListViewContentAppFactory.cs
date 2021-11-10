using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.ContentApps
{
    public class ContentTypeListViewContentAppFactory : IContentAppFactory
    {
        private const int Weight = -180;

        private ContentApp _contentTypeApp;

        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            switch (source)
            {
                case IContentType _:
                    return _contentTypeApp ?? (_contentTypeApp = new ContentApp()
                    {
                        Alias = "listView",
                        Name = "List view",
                        Icon = "icon-list",
                        View = "views/documentTypes/views/listview/listview.html",
                        Weight = Weight
                    });
                default:
                    return null;
            }
        }
    }
}
