using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Routing;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Tests.Common;
using Sbn.Tests.TestHelpers;

namespace Sbn.Tests.Routing
{
    public abstract class BaseUrlProviderTest : BaseWebTest
    {
        protected ISbnContextAccessor SbnContextAccessor { get; } = new TestSbnContextAccessor();

        protected abstract bool HideTopLevelNodeFromPath { get; }

        protected override void Compose()
        {
            base.Compose();
            Builder.Services.AddTransient<ISiteDomainMapper, SiteDomainMapper>();
        }

        protected override void ComposeSettings()
        {
            var contentSettings = new ContentSettings();
            var userPasswordConfigurationSettings = new UserPasswordConfigurationSettings();

            Builder.Services.AddTransient(x => Microsoft.Extensions.Options.Options.Create(contentSettings));
            Builder.Services.AddTransient(x => Microsoft.Extensions.Options.Options.Create(userPasswordConfigurationSettings));
        }

        protected IPublishedUrlProvider GetPublishedUrlProvider(ISbnContext sbnContext, DefaultUrlProvider urlProvider)
        {
            var webRoutingSettings = new WebRoutingSettings();
            return new UrlProvider(
                new TestSbnContextAccessor(sbnContext),
                Microsoft.Extensions.Options.Options.Create(webRoutingSettings),
                new UrlProviderCollection(new[] { urlProvider }),
                new MediaUrlProviderCollection(Enumerable.Empty<IMediaUrlProvider>()),
                Mock.Of<IVariationContextAccessor>());
        }
    }
}
