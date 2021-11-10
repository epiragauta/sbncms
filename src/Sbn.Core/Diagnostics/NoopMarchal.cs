using System;

namespace Sbn.Cms.Core.Diagnostics
{
    internal class NoopMarchal : IMarchal
    {
        public IntPtr GetExceptionPointers() => IntPtr.Zero;
    }
}
