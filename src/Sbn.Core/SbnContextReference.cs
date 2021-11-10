using System;
using Sbn.Cms.Core.Web;

namespace Sbn.Cms.Core
{
    /// <summary>
    /// Represents a reference to an <see cref="SbnContext"/> instance.
    /// </summary>
    /// <remarks>
    /// <para>A reference points to an <see cref="SbnContext"/> and it may own it (when it
    /// is a root reference) or just reference it. A reference must be disposed after it has
    /// been used. Disposing does nothing if the reference is not a root reference. Otherwise,
    /// it disposes the <see cref="SbnContext"/> and clears the
    /// <see cref="ISbnContextAccessor"/>.</para>
    /// </remarks>
    public class SbnContextReference : IDisposable
    {
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private bool _disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="SbnContextReference"/> class.
        /// </summary>
        public SbnContextReference(ISbnContext sbnContext, bool isRoot, ISbnContextAccessor sbnContextAccessor)
        {
            IsRoot = isRoot;

            SbnContext = sbnContext;
            _sbnContextAccessor = sbnContextAccessor;
        }

        /// <summary>
        /// Gets the <see cref="SbnContext"/>.
        /// </summary>
        public ISbnContext SbnContext { get; }

        /// <summary>
        /// Gets a value indicating whether the reference is a root reference.
        /// </summary>
        public bool IsRoot { get; }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (IsRoot)
                    {
                        SbnContext.Dispose();
                        _sbnContextAccessor.Clear();
                    }
                }

                _disposedValue = true;
            }
        }

        public void Dispose() => Dispose(disposing: true);
    }
}
