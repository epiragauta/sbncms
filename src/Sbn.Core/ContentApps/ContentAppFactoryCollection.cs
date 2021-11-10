using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Membership;
using Sbn.Cms.Core.Security;
using Sbn.Extensions;

namespace Sbn.Cms.Core.ContentApps
{
    public class ContentAppFactoryCollection : BuilderCollectionBase<IContentAppFactory>
    {
        private readonly ILogger<ContentAppFactoryCollection> _logger;
        private readonly IBackOfficeSecurityAccessor _backOfficeSecurityAccessor;

        public ContentAppFactoryCollection(Func<IEnumerable<IContentAppFactory>> items, ILogger<ContentAppFactoryCollection> logger, IBackOfficeSecurityAccessor backOfficeSecurityAccessor)
            : base(items)
        {
            _logger = logger;
            _backOfficeSecurityAccessor = backOfficeSecurityAccessor;
        }

        private IEnumerable<IReadOnlyUserGroup> GetCurrentUserGroups()
        {
            var currentUser = _backOfficeSecurityAccessor?.BackOfficeSecurity?.CurrentUser;
            return currentUser == null
                ? Enumerable.Empty<IReadOnlyUserGroup>()
                : currentUser.Groups;

        }

        public IEnumerable<ContentApp> GetContentAppsFor(object o, IEnumerable<IReadOnlyUserGroup> userGroups=null)
        {
            var roles = GetCurrentUserGroups();

            var apps = this.Select(x => x.GetContentAppFor(o, roles)).WhereNotNull().OrderBy(x => x.Weight).ToList();

            var aliases = new HashSet<string>();
            List<string> dups = null;

            foreach (var app in apps)
            {
                if (aliases.Contains(app.Alias))
                    (dups ?? (dups = new List<string>())).Add(app.Alias);
                else
                    aliases.Add(app.Alias);
            }

            if (dups != null)
            {
                // dying is not user-friendly, so let's write to log instead, and wish people read logs...

                //throw new InvalidOperationException($"Duplicate content app aliases found: {string.Join(",", dups)}");
                _logger.LogWarning("Duplicate content app aliases found: {DuplicateAliases}", string.Join(",", dups));
            }

            return apps;
        }
    }
}
