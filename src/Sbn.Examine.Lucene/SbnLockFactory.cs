// Copyright (c) Sbn.
// See LICENSE for more details.

using System.IO;
using Examine.Lucene.Directories;
using Lucene.Net.Store;

namespace Sbn.Cms.Infrastructure.Examine
{
    public class SbnLockFactory : ILockFactory
    {
        public LockFactory GetLockFactory(DirectoryInfo directory)
            => new NoPrefixSimpleFsLockFactory(directory);
    }
}
