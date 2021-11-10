// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Web.BackOffice.Authorization
{
    public class MediaPermissionsResource
    {
        public MediaPermissionsResource(IMedia media)
        {
            Media = media;
        }

        public MediaPermissionsResource(int nodeId)
        {
            NodeId = nodeId;
        }

        public int? NodeId { get; }
        public IMedia Media { get; }
    }
}
