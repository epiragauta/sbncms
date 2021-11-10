// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace Sbn.Cms.Tests.Integration.TestServerTest
{
    public class SbnWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly Func<IHostBuilder> _createHostBuilder;
        private readonly Action<IHost> _beforeStart;

        /// <summary>
        /// Constructor to create a new WebApplicationFactory
        /// </summary>
        /// <param name="createHostBuilder">Method to create the IHostBuilder</param>
        /// <param name="beforeStart">Method to perform an action before IHost starts</param>
        public SbnWebApplicationFactory(Func<IHostBuilder> createHostBuilder, Action<IHost> beforeStart = null)
        {
            _createHostBuilder = createHostBuilder;
            _beforeStart = beforeStart;
        }

        protected override IHostBuilder CreateHostBuilder() => _createHostBuilder();

        protected override IHost CreateHost(IHostBuilder builder)
        {
            IHost host = builder.Build();

            _beforeStart?.Invoke(host);

            host.Start();

            return host;
        }
    }
}
