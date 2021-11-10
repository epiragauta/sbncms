using System;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.PublishedCache;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    public class PublishedStatusController : SbnAuthorizedApiController
    {
        private readonly IPublishedSnapshotStatus _publishedSnapshotStatus;

        public PublishedStatusController(IPublishedSnapshotStatus publishedSnapshotStatus)
        {
            _publishedSnapshotStatus = publishedSnapshotStatus ?? throw new ArgumentNullException(nameof(publishedSnapshotStatus));
        }

        [HttpGet]
        public string GetPublishedStatusUrl()
        {
            if (!string.IsNullOrWhiteSpace(_publishedSnapshotStatus.StatusUrl))
            {
                return _publishedSnapshotStatus.StatusUrl;
            }

            throw new NotSupportedException("Not supported: " + _publishedSnapshotStatus.GetType().FullName);
        }
    }
}
