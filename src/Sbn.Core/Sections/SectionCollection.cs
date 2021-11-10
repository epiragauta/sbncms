using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.Sections
{
    public class SectionCollection : BuilderCollectionBase<ISection>
    {
        public SectionCollection(Func<IEnumerable<ISection>> items) : base(items)
        {
        }
    }
}
