using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Runtime;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.Integration
{
    [TestFixture]
    [SbnTest(Boot = true)]
    public class ComponentRuntimeTests : SbnIntegrationTest
    {
        // ensure composers are added
        protected override void CustomTestSetup(ISbnBuilder builder)
        {
            builder.AddComposers();
        }

        /// <summary>
        /// This will boot up sbn with components enabled to show they initialize and shutdown
        /// </summary>
        [Test]
        public async Task Start_And_Stop_Sbn_With_Components_Enabled()
        {
            IRuntime runtime = Services.GetRequiredService<IRuntime>();
            IRuntimeState runtimeState = Services.GetRequiredService<IRuntimeState>();
            IMainDom mainDom = Services.GetRequiredService<IMainDom>();
            ComponentCollection components = Services.GetRequiredService<ComponentCollection>();

            MyComponent myComponent = components.OfType<MyComponent>().First();

            Assert.IsTrue(mainDom.IsMainDom);
            Assert.IsNull(runtimeState.BootFailedException);
            Assert.IsTrue(myComponent.IsInit, "The component was not initialized");

            // force stop now
            await runtime.StopAsync(CancellationToken.None);
            Assert.IsTrue(myComponent.IsTerminated, "The component was not terminated");
        }

        public class MyComposer : IComposer
        {
            public void Compose(ISbnBuilder builder) => builder.Components().Append<MyComponent>();
        }

        public class MyComponent : IComponent
        {
            public bool IsInit { get; private set; }

            public bool IsTerminated { get; private set; }

            private readonly ILogger<MyComponent> _logger;

            public MyComponent(ILogger<MyComponent> logger) => _logger = logger;

            public void Initialize() => IsInit = true;

            public void Terminate() => IsTerminated = true;

        }
    }
}
