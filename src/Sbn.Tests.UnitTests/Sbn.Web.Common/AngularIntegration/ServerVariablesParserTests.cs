// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.WebAssets;
using Sbn.Cms.Infrastructure.WebAssets;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.AngularIntegration
{
    [TestFixture]
    public class ServerVariablesParserTests
    {
        [Test]
        public async Task Parse()
        {
            var parser = new ServerVariablesParser(Mock.Of<IEventAggregator>());

            var d = new Dictionary<string, object>
            {
                { "test1", "Test 1" },
                { "test2", "Test 2" },
                { "test3", "Test 3" },
                { "test4", "Test 4" },
                { "test5", "Test 5" }
            };

            var output = (await parser.ParseAsync(d)).StripWhitespace();

            Assert.IsTrue(output.Contains(@"Sbn.Sys.ServerVariables = {
  ""test1"": ""Test 1"",
  ""test2"": ""Test 2"",
  ""test3"": ""Test 3"",
  ""test4"": ""Test 4"",
  ""test5"": ""Test 5""
} ;".StripWhitespace()));
        }
    }
}
