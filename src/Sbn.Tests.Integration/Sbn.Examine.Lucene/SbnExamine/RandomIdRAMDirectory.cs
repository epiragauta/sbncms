using System;
using Lucene.Net.Store;

namespace Sbn.Cms.Tests.Integration.Sbn.Examine.Lucene.SbnExamine
{
    public class RandomIdRAMDirectory : RAMDirectory
    {
        private readonly string _lockId = Guid.NewGuid().ToString();
        public override string GetLockID() => _lockId;
    }
}
