using System;

namespace Sbn.Cms.Core
{
    public interface ICompletable : IDisposable
    {
        void Complete();
    }
}
