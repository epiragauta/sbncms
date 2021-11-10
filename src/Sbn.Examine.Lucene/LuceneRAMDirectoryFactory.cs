// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.IO;
using System.Threading;
using Examine.Lucene.Directories;
using Examine.Lucene.Providers;
using Lucene.Net.Store;
using Directory = Lucene.Net.Store.Directory;

namespace Sbn.Cms.Infrastructure.Examine
{
    public class LuceneRAMDirectoryFactory : DirectoryFactoryBase
    {

        public LuceneRAMDirectoryFactory()
        {
        }

        protected override Directory CreateDirectory(LuceneIndex luceneIndex, bool forceUnlock)
            => new RandomIdRAMDirectory();

        private class RandomIdRAMDirectory : RAMDirectory
        {
            private readonly string _lockId = Guid.NewGuid().ToString();
            public override string GetLockID() => _lockId;
        }
    }
}
