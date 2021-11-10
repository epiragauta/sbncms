using Sbn.Cms.Core.Routing;

namespace Sbn.Tests.TestHelpers.Stubs
{
    internal class TestLastChanceFinder : IContentLastChanceFinder
    {
        public bool TryFindContent(IPublishedRequestBuilder frequest) => false;
    }
}
