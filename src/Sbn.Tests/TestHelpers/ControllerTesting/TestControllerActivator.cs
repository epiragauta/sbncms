using System;
using System.Net.Http;
using System.Web.Http;
using Sbn.Cms.Core.Web;
using Sbn.Web;

namespace Sbn.Tests.TestHelpers.ControllerTesting
{
    public class TestControllerActivator : TestControllerActivatorBase
    {
        private readonly Func<HttpRequestMessage, ISbnContextAccessor, ApiController> _factory;

        public TestControllerActivator(Func<HttpRequestMessage, ISbnContextAccessor, ApiController> factory)
        {
            _factory = factory;
        }

        protected override ApiController CreateController(Type controllerType, HttpRequestMessage msg, ISbnContextAccessor sbnContextAccessor)
        {
            return _factory(msg, sbnContextAccessor);
        }
    }
}
