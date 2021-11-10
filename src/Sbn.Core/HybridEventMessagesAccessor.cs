using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Events;

namespace Sbn.Cms.Core
{
    public class HybridEventMessagesAccessor : HybridAccessorBase<EventMessages>, IEventMessagesAccessor
    {
        public HybridEventMessagesAccessor(IRequestCache requestCache)
            : base(requestCache)
        { }

        public EventMessages EventMessages
        {
            get { return Value; }
            set { Value = value; }
        }
    }
}
