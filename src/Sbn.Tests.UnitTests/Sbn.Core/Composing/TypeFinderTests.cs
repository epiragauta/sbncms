// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Web.BackOffice.Trees;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Composing
{
    /// <summary>
    /// Tests for typefinder
    /// </summary>
    [TestFixture]
    public class TypeFinderTests
    {
        /// <summary>
        /// List of assemblies to scan
        /// </summary>
        private Assembly[] _assemblies;

        [SetUp]
        public void Initialize() => _assemblies = new[]
                {
                    GetType().Assembly,
                    typeof(System.Guid).Assembly,
                    typeof(NUnit.Framework.Assert).Assembly,
                    typeof(System.Xml.NameTable).Assembly,
                    typeof(TypeFinder).Assembly,
                };

        [Test]
        public void Find_Class_Of_Type_With_Attribute()
        {
            var typeFinder = new TypeFinder(Mock.Of<ILogger<TypeFinder>>(), new DefaultSbnAssemblyProvider(GetType().Assembly, NullLoggerFactory.Instance));
            IEnumerable<Type> typesFound = typeFinder.FindClassesOfTypeWithAttribute<TestEditor, MyTestAttribute>(_assemblies);
            Assert.AreEqual(2, typesFound.Count());
        }

        [Test]
        public void Find_Classes_With_Attribute()
        {
            var typeFinder = new TypeFinder(Mock.Of<ILogger<TypeFinder>>(), new DefaultSbnAssemblyProvider(GetType().Assembly, NullLoggerFactory.Instance));
            IEnumerable<Type> typesFound = typeFinder.FindClassesWithAttribute<TreeAttribute>(_assemblies);
            Assert.AreEqual(0, typesFound.Count()); // 0 classes in _assemblies are marked with [Tree]

            typesFound = typeFinder.FindClassesWithAttribute<TreeAttribute>(new[] { typeof(TreeAttribute).Assembly });
            Assert.AreEqual(23, typesFound.Count()); // + classes in Sbn.Web are marked with [Tree]

            typesFound = typeFinder.FindClassesWithAttribute<TreeAttribute>();
            Assert.AreEqual(23, typesFound.Count()); // + classes in Sbn.Web are marked with [Tree]
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        public class MyTestAttribute : Attribute
        {
        }

        public abstract class TestEditor
        {
        }

        [MyTest]
        public class BenchmarkTestEditor : TestEditor
        {
        }

        [MyTest]
        public class MyOtherTestEditor : TestEditor
        {
        }
    }
}
