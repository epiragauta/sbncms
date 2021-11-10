// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using NUnit.Framework;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Persistence
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class UnitOfWorkTests : SbnIntegrationTest
    {
        [Test]
        public void ReadLockNonExisting()
        {
            IScopeProvider provider = ScopeProvider;
            Assert.Throws<ArgumentException>(() =>
            {
                using (IScope scope = provider.CreateScope())
                {
                    scope.EagerReadLock(-666);
                    scope.Complete();
                }
            });
        }

        [Test]
        public void ReadLockExisting()
        {
            IScopeProvider provider = ScopeProvider;
            using (IScope scope = provider.CreateScope())
            {
                scope.EagerReadLock(Constants.Locks.Servers);
                scope.Complete();
            }
        }

        [Test]
        public void WriteLockNonExisting()
        {
            IScopeProvider provider = ScopeProvider;
            Assert.Throws<ArgumentException>(() =>
            {
                using (IScope scope = provider.CreateScope())
                {
                    scope.EagerWriteLock(-666);
                    scope.Complete();
                }
            });
        }

        [Test]
        public void WriteLockExisting()
        {
            IScopeProvider provider = ScopeProvider;
            using (IScope scope = provider.CreateScope())
            {
                scope.EagerWriteLock(Constants.Locks.Servers);
                scope.Complete();
            }
        }
    }
}
