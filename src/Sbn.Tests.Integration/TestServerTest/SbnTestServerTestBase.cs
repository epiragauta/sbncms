// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Linq.Expressions;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.DependencyInjection;
using Sbn.Cms.Tests.Integration.Testing;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Cms.Web.UI;
using Sbn.Cms.Web.Website.Controllers;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration.TestServerTest
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest, Logger = SbnTestOptions.Logger.Console, Boot = true)]
    public abstract class SbnTestServerTestBase : SbnIntegrationTest
    {
        [SetUp]
        public override void Setup()
        {
            InMemoryConfiguration["ConnectionStrings:" + Constants.System.SbnConnectionName] = null;
            InMemoryConfiguration["Sbn:CMS:Hosting:Debug"] = "true";

            /*
             * It's worth noting that our usage of WebApplicationFactory is non-standard,
             * the intent is that your Startup.ConfigureServices is called just like
             * when the app starts up, then replacements are registered in this class with
             * builder.ConfigureServices (builder.ConfigureTestServices has hung around from before the
             * generic host switchover).
             *
             * This is currently a pain to refactor towards due to SbnBuilder+TypeFinder+TypeLoader setup but
             * we should get there one day.
             *
             * See https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests
             */
            var factory = new SbnWebApplicationFactory<Startup>(CreateHostBuilder, BeforeHostStart);

            // additional host configuration for web server integration tests
            Factory = factory.WithWebHostBuilder(builder =>

                // Executes after the standard ConfigureServices method
                builder.ConfigureTestServices(services =>

                    // Add a test auth scheme with a test auth handler to authn and assign the user
                    services.AddAuthentication(TestAuthHandler.TestAuthenticationScheme)
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.TestAuthenticationScheme, options => { })));

            Client = Factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            LinkGenerator = Factory.Services.GetRequiredService<LinkGenerator>();
        }

        public override IHostBuilder CreateHostBuilder()
        {
            IHostBuilder builder = base.CreateHostBuilder();
            builder.ConfigureWebHost(builder =>
            {
                 // need to configure the IWebHostEnvironment too
                 builder.ConfigureServices((c, s) => c.HostingEnvironment = TestHelper.GetWebHostEnvironment());

                 // call startup
                 builder.Configure(app => Configure(app));
            }).UseEnvironment(Environments.Development);

            return builder;
        }

        /// <summary>
        /// Prepare a url before using <see cref="Client"/>.
        /// This returns the url but also sets the HttpContext.request into to use this url.
        /// </summary>
        /// <returns>The string URL of the controller action.</returns>
        protected string PrepareApiControllerUrl<T>(Expression<Func<T, object>> methodSelector)
            where T : SbnApiController
        {
            string url = LinkGenerator.GetSbnApiService(methodSelector);
            return PrepareUrl(url);
        }

        /// <summary>
        /// Prepare a url before using <see cref="Client"/>.
        /// This returns the url but also sets the HttpContext.request into to use this url.
        /// </summary>
        /// <returns>The string URL of the controller action.</returns>
        protected string PrepareSurfaceControllerUrl<T>(Expression<Func<T, object>> methodSelector)
            where T : SurfaceController
        {
            string url = LinkGenerator.GetSbnSurfaceUrl(methodSelector);
            return PrepareUrl(url);
        }

        /// <summary>
        /// Prepare a url before using <see cref="Client"/>.
        /// This returns the url but also sets the HttpContext.request into to use this url.
        /// </summary>
        /// <returns>The string URL of the controller action.</returns>
        protected string PrepareUrl(string url)
        {
            ISbnContextFactory sbnContextFactory = GetRequiredService<ISbnContextFactory>();
            IHttpContextAccessor httpContextAccessor = GetRequiredService<IHttpContextAccessor>();

            httpContextAccessor.HttpContext = new DefaultHttpContext
            {
                Request =
                {
                    Scheme = "https",
                    Host = new HostString("localhost", 80),
                    Path = url,
                    QueryString = new QueryString(string.Empty)
                }
            };

            sbnContextFactory.EnsureSbnContext();

            return url;
        }

        protected HttpClient Client { get; private set; }

        protected LinkGenerator LinkGenerator { get; private set; }

        protected WebApplicationFactory<Startup> Factory { get; private set; }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TestSbnDatabaseFactoryProvider>();

            Core.Hosting.IHostingEnvironment hostingEnvironment = TestHelper.GetHostingEnvironment();
            TypeLoader typeLoader = services.AddTypeLoader(
                GetType().Assembly,
                hostingEnvironment,
                TestHelper.ConsoleLoggerFactory,
                AppCaches.NoCache,
                Configuration,
                TestHelper.Profiler);

            var builder = new SbnBuilder(services, Configuration, typeLoader, TestHelper.ConsoleLoggerFactory, TestHelper.Profiler, AppCaches.NoCache, hostingEnvironment);

            builder
                .AddConfiguration()
                .AddSbnCore()
                .AddWebComponents()
                .AddNuCache()
                .AddRuntimeMinifier()
                .AddBackOfficeCore()
                .AddBackOfficeAuthentication()
                .AddBackOfficeIdentity()
                .AddMembersIdentity()
                .AddBackOfficeAuthorizationPolicies(TestAuthHandler.TestAuthenticationScheme)
                .AddPreviewSupport()
                .AddMvcAndRazor(mvcBuilding: mvcBuilder =>
                {
                    // Adds Sbn.Web.BackOffice
                    mvcBuilder.AddApplicationPart(typeof(ContentController).Assembly);

                    // Adds Sbn.Web.Common
                    mvcBuilder.AddApplicationPart(typeof(RenderController).Assembly);

                    // Adds Sbn.Web.Website
                    mvcBuilder.AddApplicationPart(typeof(SurfaceController).Assembly);

                    // Adds Sbn.Tests.Integration
                    mvcBuilder.AddApplicationPart(typeof(SbnTestServerTestBase).Assembly);
                })
                .AddWebServer()
                .AddWebsite()
                .AddTestServices(TestHelper) // This is the important one!
                .Build();
        }

        public override void Configure(IApplicationBuilder app)
        {
            app.UseSbn()
                .WithMiddleware(u =>
                {
                    u.UseBackOffice();
                    u.UseWebsite();
                })
                .WithEndpoints(u =>
                {
                    u.UseBackOfficeEndpoints();
                    u.UseWebsiteEndpoints();
                });
        }
    }
}
