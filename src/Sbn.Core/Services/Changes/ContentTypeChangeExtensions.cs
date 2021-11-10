// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Services.Changes;

namespace Sbn.Extensions
{
    public static class ContentTypeChangeExtensions
    {

        public static bool HasType(this ContentTypeChangeTypes change, ContentTypeChangeTypes type)
        {
            return (change & type) != ContentTypeChangeTypes.None;
        }

        public static bool HasTypesAll(this ContentTypeChangeTypes change, ContentTypeChangeTypes types)
        {
            return (change & types) == types;
        }

        public static bool HasTypesAny(this ContentTypeChangeTypes change, ContentTypeChangeTypes types)
        {
            return (change & types) != ContentTypeChangeTypes.None;
        }

        public static bool HasTypesNone(this ContentTypeChangeTypes change, ContentTypeChangeTypes types)
        {
            return (change & types) == ContentTypeChangeTypes.None;
        }
    }
}
