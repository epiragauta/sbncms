// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Models.Entities;
using Sbn.Cms.Tests.Common.Builders;
using Sbn.Cms.Tests.Common.Builders.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Infrastructure.Models
{
    [TestFixture]
    public class PathValidationTests
    {
        private EntitySlimBuilder _builder;

        [SetUp]
        public void SetUp() => _builder = new EntitySlimBuilder();

        [Test]
        public void Validate_Path()
        {
            EntitySlim entity = _builder
                .WithoutIdentity()
                .Build();

            // it's empty with no id so we need to allow it
            Assert.IsTrue(entity.ValidatePath());

            entity.Id = 1234;

            // it has an id but no path, so we can't allow it
            Assert.IsFalse(entity.ValidatePath());

            entity.Path = "-1";

            // invalid path
            Assert.IsFalse(entity.ValidatePath());

            entity.Path = string.Concat("-1,", entity.Id);

            // valid path
            Assert.IsTrue(entity.ValidatePath());
        }

        [Test]
        public void Ensure_Path_Throws_Without_Id()
        {
            EntitySlim entity = _builder
                .WithoutIdentity()
                .Build();

            // no id assigned
            Assert.Throws<InvalidOperationException>(() => entity.EnsureValidPath(Mock.Of<ILogger<EntitySlim>>(), sbnEntity => new EntitySlim(), sbnEntity => { }));
        }

        [Test]
        public void Ensure_Path_Throws_Without_Parent()
        {
            EntitySlim entity = _builder
                .WithId(1234)
                .WithNoParentId()
                .Build();

            // no parent found
            Assert.Throws<NullReferenceException>(() => entity.EnsureValidPath(Mock.Of<ILogger<EntitySlim>>(), sbnEntity => null, sbnEntity => { }));
        }

        [Test]
        public void Ensure_Path_Entity_At_Root()
        {
            EntitySlim entity = _builder
                .WithId(1234)
                .Build();

            entity.EnsureValidPath(Mock.Of<ILogger<EntitySlim>>(), sbnEntity => null, sbnEntity => { });

            // works because it's under the root
            Assert.AreEqual("-1,1234", entity.Path);
        }

        [Test]
        public void Ensure_Path_Entity_Valid_Parent()
        {
            EntitySlim entity = _builder
                .WithId(1234)
                .WithParentId(888)
                .Build();

            entity.EnsureValidPath(Mock.Of<ILogger<EntitySlim>>(), sbnEntity => sbnEntity.ParentId == 888 ? new EntitySlim { Id = 888, Path = "-1,888" } : null, sbnEntity => { });

            // works because the parent was found
            Assert.AreEqual("-1,888,1234", entity.Path);
        }

        [Test]
        public void Ensure_Path_Entity_Valid_Recursive_Parent()
        {
            EntitySlim parentA = _builder
                .WithId(999)
                .Build();

            // Re-creating the class-level builder as we need to reset before usage when creating multiple entities.
            _builder = new EntitySlimBuilder();
            EntitySlim parentB = _builder
                .WithId(888)
                .WithParentId(999)
                .Build();

            _builder = new EntitySlimBuilder();
            EntitySlim parentC = _builder
                .WithId(777)
                .WithParentId(888)
                .Build();

            _builder = new EntitySlimBuilder();
            EntitySlim entity = _builder
                .WithId(1234)
                .WithParentId(777)
                .Build();

            ISbnEntity GetParent(ISbnEntity sbnEntity)
            {
                switch (sbnEntity.ParentId)
                {
                    case 999:
                        return parentA;
                    case 888:
                        return parentB;
                    case 777:
                        return parentC;
                    case 1234:
                        return entity;
                    default:
                        return null;
                }
            }

            // this will recursively fix all paths
            entity.EnsureValidPath(Mock.Of<ILogger<ISbnEntity>>(), GetParent, sbnEntity => { });

            Assert.AreEqual("-1,999", parentA.Path);
            Assert.AreEqual("-1,999,888", parentB.Path);
            Assert.AreEqual("-1,999,888,777", parentC.Path);
            Assert.AreEqual("-1,999,888,777,1234", entity.Path);
        }
    }
}
