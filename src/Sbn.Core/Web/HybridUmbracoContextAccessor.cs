using Sbn.Cms.Core.Cache;

namespace Sbn.Cms.Core.Web
{
    /// <summary>
    /// Implements a hybrid <see cref="ISbnContextAccessor"/>.
    /// </summary>
    public class HybridSbnContextAccessor : HybridAccessorBase<ISbnContext>, ISbnContextAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HybridSbnContextAccessor"/> class.
        /// </summary>
        public HybridSbnContextAccessor(IRequestCache requestCache)
            : base(requestCache)
        { }

        /// <summary>
        /// Tries to get the <see cref="SbnContext"/> object.
        /// </summary>
        public bool TryGetSbnContext(out ISbnContext sbnContext)
        {
            sbnContext = Value;

            return sbnContext is not null;
        }

        /// <summary>
        /// Clears the current <see cref="SbnContext"/> object.
        /// </summary>
        public void Clear() => Value = null;

        /// <summary>
        /// Sets the <see cref="SbnContext"/> object.
        /// </summary>
        /// <param name="sbnContext"></param>
        public void Set(ISbnContext sbnContext) => Value = sbnContext;
    }
}
