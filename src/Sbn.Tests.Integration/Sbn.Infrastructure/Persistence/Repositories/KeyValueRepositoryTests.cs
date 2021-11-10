// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Persistence.Repositories;
using Sbn.Cms.Core.Scoping;
using Sbn.Cms.Infrastructure.Persistence.Repositories.Implement;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Persistence.Repositories
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class KeyValueRepositoryTests : SbnIntegrationTest
    {
        [Test]
        public void CanSetAndGet()
        {
            IScopeProvider provider = ScopeProvider;

            // Insert new key/value
            using (IScope scope = provider.CreateScope())
            {
                var keyValue = new KeyValue
                {
                    Identifier = "foo",
                    Value = "bar",
                    UpdateDate = DateTime.Now,
                };
                IKeyValueRepository repo = CreateRepository(provider);
                repo.Save(keyValue);
                scope.Complete();
            }

            // Retrieve key/value
            using (IScope scope = provider.CreateScope())
            {
                IKeyValueRepository repo = CreateRepository(provider);
                IKeyValue keyValue = repo.Get("foo");
                scope.Complete();

                Assert.AreEqual("bar", keyValue.Value);
            }

            // Update value
            using (IScope scope = provider.CreateScope())
            {
                IKeyValueRepository repo = CreateRepository(provider);
                IKeyValue keyValue = repo.Get("foo");
                keyValue.Value = "buzz";
                keyValue.UpdateDate = DateTime.Now;
                repo.Save(keyValue);
                scope.Complete();
            }

            // Retrieve key/value again
            using (IScope scope = provider.CreateScope())
            {
                IKeyValueRepository repo = CreateRepository(provider);
                IKeyValue keyValue = repo.Get("foo");
                scope.Complete();

                Assert.AreEqual("buzz", keyValue.Value);
            }
        }

        private IKeyValueRepository CreateRepository(IScopeProvider provider) => new KeyValueRepository((IScopeAccessor)provider, LoggerFactory.CreateLogger<KeyValueRepository>());
    }
}
