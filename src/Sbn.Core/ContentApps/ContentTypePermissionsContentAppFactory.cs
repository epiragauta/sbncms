using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Membership;

namespace Sbn.Cms.Core.ContentApps
{
    public class ContentTypePermissionsContentAppFactory : IContentAppFactory
    {
        private const int Weight = -160;

        private ContentApp _contentTypeApp;

        public ContentApp GetContentAppFor(object source, IEnumerable<IReadOnlyUserGroup> userGroups)
        {
            switch (source)
            {
                case IContentType _:
                    return _contentTypeApp ?? (_contentTypeApp = new ContentApp()
                    {
                        Alias = "permissions",
                        Name = "Permissions",
                        Icon = "icon-keychain",
                        View = "views/documentTypes/views/permissions/permissions.html",
                        Weight = Weight
                    });
                default:
                    return null;
            }
        }
    }
}
