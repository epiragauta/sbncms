// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Diagnostics;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Logging;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Net;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Cms.Infrastructure.Serialization;
using Sbn.Cms.Tests.Common.TestHelpers;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.Common
{
    /// <summary>
    /// Common helper properties and methods useful to testing
    /// </summary>
    public abstract class TestHelperBase
    {
        private readonly ITypeFinder _typeFinder;
        private UriUtility _uriUtility;
        private IIOHelper _ioHelper;
        private string _workingDir;

        protected TestHelperBase(Assembly entryAssembly)
        {
            MainDom = new SimpleMainDom();
            _typeFinder = new TypeFinder(NullLoggerFactory.Instance.CreateLogger<TypeFinder>(), new DefaultSbnAssemblyProvider(entryAssembly, NullLoggerFactory.Instance));
        }

        public ITypeFinder GetTypeFinder() => _typeFinder;

        public TypeLoader GetMockedTypeLoader() =>
            new TypeLoader(Mock.Of<ITypeFinder>(), new VaryingRuntimeHash(), Mock.Of<IAppPolicyCache>(), new DirectoryInfo(GetHostingEnvironment().MapPathContentRoot(Constants.SystemDirectories.TempData)), Mock.Of<ILogger<TypeLoader>>(), Mock.Of<IProfiler>());

        /// <summary>
        /// Gets the working directory of the test project.
        /// </summary>
        public string WorkingDirectory
        {
            get
            {
                if (_workingDir != null)
                {
                    return _workingDir;
                }

                // Azure DevOps can only store a database in certain locations so we will need to detect if we are running
                // on a build server and if so we'll use the temp path.
                var dir = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("System_DefaultWorkingDirectory"))
                    ? Path.Combine(Assembly.GetExecutingAssembly().GetRootDirectorySafe(), "TEMP")
                    : Path.Combine(Path.GetTempPath(), "SbnTests", "TEMP");

                if (!Directory.Exists(dir))
                {
                    _ = Directory.CreateDirectory(dir);
                }

                _workingDir = dir;
                return _workingDir;
            }
        }

        public IShortStringHelper ShortStringHelper { get; } = new DefaultShortStringHelper(new DefaultShortStringHelperConfig());

        public IJsonSerializer JsonSerializer { get; } = new JsonNetSerializer();

        public IVariationContextAccessor VariationContextAccessor { get; } = new TestVariationContextAccessor();

        public abstract IDbProviderFactoryCreator DbProviderFactoryCreator { get; }

        public abstract IBulkSqlInsertProvider BulkSqlInsertProvider { get; }

        public abstract IMarchal Marchal { get; }

        public CoreDebugSettings CoreDebugSettings { get; } = new CoreDebugSettings();

        public IIOHelper IOHelper
        {
            get
            {
                if (_ioHelper == null)
                {
                    IHostingEnvironment hostingEnvironment = GetHostingEnvironment();

                    if (TestEnvironment.IsWindows)
                    {
                        _ioHelper = new IOHelperWindows(hostingEnvironment);
                    }
                    else if (TestEnvironment.IsLinux)
                    {
                        _ioHelper = new IOHelperLinux(hostingEnvironment);
                    }
                    else if (TestEnvironment.IsOSX)
                    {
                        _ioHelper = new IOHelperOSX(hostingEnvironment);
                    }
                    else
                    {
                        throw new NotSupportedException("Unexpected OS");
                    }
                }

                return _ioHelper;
            }
        }

        public IMainDom MainDom { get; }

        public UriUtility UriUtility
        {
            get
            {
                if (_uriUtility == null)
                {
                    _uriUtility = new UriUtility(GetHostingEnvironment());
                }

                return _uriUtility;
            }
        }

        /// <summary>
        /// Some test files are copied to the /bin (/bin/debug) on build, this is a utility to return their physical path based on a virtual path name
        /// </summary>
        public virtual string MapPathForTestFiles(string relativePath)
        {
            if (!relativePath.StartsWith("~/"))
            {
                throw new ArgumentException("relativePath must start with '~/'", nameof(relativePath));
            }

            var codeBase = typeof(TestHelperBase).Assembly.CodeBase;
            var uri = new Uri(codeBase);
            var path = uri.LocalPath;
            var bin = Path.GetDirectoryName(path);

            return relativePath.Replace("~/", bin + "/");
        }

        public ISbnVersion GetSbnVersion() => new SbnVersion();

        public IServiceCollection GetRegister() => new ServiceCollection();

        public abstract IHostingEnvironment GetHostingEnvironment();

        public abstract IApplicationShutdownRegistry GetHostingEnvironmentLifetime();

        public abstract IIpResolver GetIpResolver();

        public IRequestCache GetRequestCache() => new DictionaryAppCache();

        public IPublishedUrlProvider GetPublishedUrlProvider()
        {
            var mock = new Mock<IPublishedUrlProvider>();

            return mock.Object;
        }

        public ILoggingConfiguration GetLoggingConfiguration(IHostingEnvironment hostingEnv = null)
        {
            hostingEnv = hostingEnv ?? GetHostingEnvironment();
            return new LoggingConfiguration(
                Path.Combine(hostingEnv.ApplicationPhysicalPath, "sbn", "logs"));
        }
    }
}
