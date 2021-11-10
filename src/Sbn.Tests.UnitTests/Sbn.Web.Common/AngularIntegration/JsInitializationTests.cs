// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Infrastructure.WebAssets;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.AngularIntegration
{
    [TestFixture]
    public class JsInitializationTests
    {
        [Test]
        public void Parse_Main()
        {
            var result = BackOfficeJavaScriptInitializer.WriteScript("[World]", "Hello", "Blah");

            Assert.AreEqual(
                @"LazyLoad.js([World], function () {
    //we need to set the legacy UmbClientMgr path
    if ((typeof UmbClientMgr) !== ""undefined"") {
        UmbClientMgr.setSbnPath('Hello');
    }

    jQuery(document).ready(function () {

        angular.bootstrap(document, ['Blah']);

    });
});".StripWhitespace(), result.StripWhitespace());
        }
    }
}
