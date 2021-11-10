// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Tests.UnitTests.TestHelpers;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Composing
{
    public abstract class ComposingTestBase
    {
        protected TypeLoader TypeLoader { get; private set; }

        [SetUp]
        public void Initialize()
        {
            var typeFinder = TestHelper.GetTypeFinder();
            TypeLoader = new TypeLoader(typeFinder, new VaryingRuntimeHash(), NoAppCache.Instance, new DirectoryInfo(TestHelper.GetHostingEnvironment().MapPathContentRoot(Constants.SystemDirectories.TempData)), Mock.Of<ILogger<TypeLoader>>(), Mock.Of<IProfiler>(), false, AssembliesToScan);
        }

        protected virtual IEnumerable<Assembly> AssembliesToScan
            => new[]
            {
                GetType().Assembly // this assembly only
            };
    }
}
