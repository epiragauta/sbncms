// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Runtime.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using Sbn.Cms.Web.Common.Formatters;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Web.Common.Formatters
{
    [TestFixture]
    public class IgnoreRequiredAttributesResolverUnitTests
    {
        [Test]
        public void Test()
        {
            const string emptyJsonObject = "{}";

            Assert.Multiple(() =>
            {
                // Ensure the deserialization throws if using default settings
                Assert.Throws<JsonSerializationException>(() =>
                    JsonConvert.DeserializeObject<ObjectWithRequiredProperty>(emptyJsonObject));

                ObjectWithRequiredProperty actual = JsonConvert.DeserializeObject<ObjectWithRequiredProperty>(
                    emptyJsonObject,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new IgnoreRequiredAttributesResolver()
                    });

                Assert.IsNotNull(actual);
                Assert.IsNull(actual.Property);
            });
        }

        [DataContract(Name = "objectWithRequiredProperty", Namespace = "")]
        private class ObjectWithRequiredProperty
        {
            [DataMember(Name = "property", IsRequired = true)]
            public string Property { get; set; }
        }
    }
}
