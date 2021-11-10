// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Linq;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Security;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.BackOffice.Install;
using Sbn.Cms.Web.BackOffice.Routing;
using Sbn.Cms.Web.Common.Security;

namespace Sbn.Cms.Tests.UnitTests.AutoFixture
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        /// <summary>
        /// Uses AutoFixture to automatically mock (using Moq) the injected types. E.g when injecting interfaces.
        /// AutoFixture is used to generate concrete types. If the concrete type required some types injected, the
        /// [Frozen] can be used to ensure the same variable is injected and available as parameter for the test
        /// </summary>
        public AutoMoqDataAttribute()
            : base(() => AutoMockCustomizations.Default)
        {
        }

        internal static class AutoMockCustomizations
        {
            public static IFixture Default => new Fixture().Customize(new SbnCustomization());

            private class SbnCustomization : ICustomization
            {
                public void Customize(IFixture fixture)
                {
                    fixture.Customize<BackOfficeIdentityUser>(
                        u => u.FromFactory<string, string, string>(
                            (a, b, c) => BackOfficeIdentityUser.CreateNew(new GlobalSettings(), a, b, c)));
                    fixture
                        .Customize(new ConstructorCustomization(typeof(UsersController), new GreedyConstructorQuery()))
                        .Customize(new ConstructorCustomization(typeof(InstallController), new GreedyConstructorQuery()))
                        .Customize(new ConstructorCustomization(typeof(PreviewController), new GreedyConstructorQuery()))
                        .Customize(new ConstructorCustomization(typeof(MemberController), new GreedyConstructorQuery()))
                        .Customize(new ConstructorCustomization(typeof(BackOfficeController), new GreedyConstructorQuery()))
                        .Customize(new ConstructorCustomization(typeof(BackOfficeUserManager), new GreedyConstructorQuery()))
                        .Customize(new ConstructorCustomization(typeof(MemberManager), new GreedyConstructorQuery()));

                    fixture.Customize(new AutoMoqCustomization());

                    // When requesting an IUserStore ensure we actually uses a IUserLockoutStore
                    fixture.Customize<IUserStore<BackOfficeIdentityUser>>(cc => cc.FromFactory(() => Mock.Of<IUserLockoutStore<BackOfficeIdentityUser>>()));

                    fixture.Customize<ConfigConnectionString>(
                        u => u.FromFactory<string, string, string>(
                            (a, b, c) => new ConfigConnectionString(a, b, c)));

                    fixture.Customize<ISbnVersion>(
                        u => u.FromFactory(
                            () => new SbnVersion()));

                    fixture.Customize<BackOfficeAreaRoutes>(u => u.FromFactory(
                        () => new BackOfficeAreaRoutes(
                            Options.Create(new GlobalSettings()),
                            Mock.Of<IHostingEnvironment>(x => x.ToAbsolute(It.IsAny<string>()) == "/sbn" && x.ApplicationVirtualPath == string.Empty),
                            Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run),
                            new SbnApiControllerTypeCollection(() => Enumerable.Empty<Type>()))));

                    fixture.Customize<PreviewRoutes>(u => u.FromFactory(
                        () => new PreviewRoutes(
                            Options.Create(new GlobalSettings()),
                            Mock.Of<IHostingEnvironment>(x => x.ToAbsolute(It.IsAny<string>()) == "/sbn" && x.ApplicationVirtualPath == string.Empty),
                            Mock.Of<IRuntimeState>(x => x.Level == RuntimeLevel.Run))));

                    var connectionStrings = new ConnectionStrings();
                    fixture.Customize<ConnectionStrings>(x => x.FromFactory(() => connectionStrings));

                    var httpContextAccessor = new HttpContextAccessor { HttpContext = new DefaultHttpContext() };
                    fixture.Customize<HttpContext>(x => x.FromFactory(() => httpContextAccessor.HttpContext));
                    fixture.Customize<IHttpContextAccessor>(x => x.FromFactory(() => httpContextAccessor));
                }
            }
        }
    }
}
