using System;
using Microsoft.AspNetCore.Http;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.PublishedCache;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Web.Common.SbnContext
{
    /// <summary>
    /// Creates and manages <see cref="ISbnContext"/> instances.
    /// </summary>
    public class SbnContextFactory : ISbnContextFactory
    {
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IPublishedSnapshotService _publishedSnapshotService;
        private readonly SbnRequestPaths _sbnRequestPaths;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ICookieManager _cookieManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UriUtility _uriUtility;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnContextFactory"/> class.
        /// </summary>
        public SbnContextFactory(
            ISbnContextAccessor sbnContextAccessor,
            IPublishedSnapshotService publishedSnapshotService,
            SbnRequestPaths sbnRequestPaths,
            IHostingEnvironment hostingEnvironment,
            UriUtility uriUtility,
            ICookieManager cookieManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _sbnContextAccessor = sbnContextAccessor ?? throw new ArgumentNullException(nameof(sbnContextAccessor));
            _publishedSnapshotService = publishedSnapshotService ?? throw new ArgumentNullException(nameof(publishedSnapshotService));
            _sbnRequestPaths = sbnRequestPaths ?? throw new ArgumentNullException(nameof(sbnRequestPaths));
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _uriUtility = uriUtility ?? throw new ArgumentNullException(nameof(uriUtility));
            _cookieManager = cookieManager ?? throw new ArgumentNullException(nameof(cookieManager));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        private ISbnContext CreateSbnContext() => new SbnContext(
                _publishedSnapshotService,
                _sbnRequestPaths,
                _hostingEnvironment,
                _uriUtility,
                _cookieManager,
                _httpContextAccessor);

        /// <inheritdoc />
        public SbnContextReference EnsureSbnContext()
        {
            if (_sbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                return new SbnContextReference(sbnContext, false, _sbnContextAccessor);
            }

            ISbnContext createdSbnContext = CreateSbnContext();

            _sbnContextAccessor.Set(createdSbnContext);
            return new SbnContextReference(createdSbnContext, true, _sbnContextAccessor);
        }

    }
}
