// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Sbn.Cms.Core.Web;

namespace Sbn.Extensions
{
    public static class SbnContextAccessorExtensions
    {
        public static ISbnContext GetRequiredSbnContext(this ISbnContextAccessor sbnContextAccessor)
        {
            if (sbnContextAccessor == null) throw new ArgumentNullException(nameof(sbnContextAccessor));
            if(!sbnContextAccessor.TryGetSbnContext(out var sbnContext))
            {
                throw new InvalidOperationException("Wasn't able to get an SbnContext");
            }
            return sbnContext;
        }
    }
}
