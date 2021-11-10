using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using Sbn.Cms.Core.Web;
using Sbn.Web;

namespace Sbn.Tests.LegacyXmlPublishedCache
{
    static class SbnContextCache
    {
        static readonly ConditionalWeakTable<ISbnContext, ConcurrentDictionary<string, object>> Caches
            = new ConditionalWeakTable<ISbnContext, ConcurrentDictionary<string, object>>();

        public static ConcurrentDictionary<string, object> Current
        {
            get
            {
                var sbnContext = Sbn.Web.Composing.Current.SbnContext;

                // will get or create a value
                // a ConditionalWeakTable is thread-safe
                // does not prevent the context from being disposed, and then the dictionary will be disposed too
                return sbnContext == null ? null : Caches.GetOrCreateValue(sbnContext);
            }
        }
    }
}
