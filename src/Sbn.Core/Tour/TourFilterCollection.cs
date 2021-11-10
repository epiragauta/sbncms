using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Tour
{
    /// <summary>
    /// Represents a collection of <see cref="BackOfficeTourFilter"/> items.
    /// </summary>
    public class TourFilterCollection : BuilderCollectionBase<BackOfficeTourFilter>
    {
        public TourFilterCollection(Func<IEnumerable<BackOfficeTourFilter>> items) : base(items)
        {
        }
    }
}
