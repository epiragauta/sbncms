// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core;
using Sbn.Cms.Core.Models;

namespace Sbn.Extensions
{
    public static class MediaTypeExtensions
    {
        public static bool IsSystemMediaType(this IMediaType mediaType) =>
            mediaType.Alias == Constants.Conventions.MediaTypes.File
            || mediaType.Alias == Constants.Conventions.MediaTypes.Folder
            || mediaType.Alias == Constants.Conventions.MediaTypes.Image;
    }
}
