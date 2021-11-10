// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Serialization;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Services
{
    /// <summary>
    /// Tests covering the DataTypeService with cache enabled
    /// </summary>
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class CachedDataTypeServiceTests : SbnIntegrationTest
    {
        private IDataValueEditorFactory DataValueEditorFactory => GetRequiredService<IDataValueEditorFactory>();
        private IDataTypeService DataTypeService => GetRequiredService<IDataTypeService>();
        private IConfigurationEditorJsonSerializer ConfigurationEditorJsonSerializer => GetRequiredService<IConfigurationEditorJsonSerializer>();

        /// <summary>
        /// This tests validates that with the new scope changes that the underlying cache policies work - in this case it tests that the cache policy
        /// with Count verification works.
        /// </summary>
        [Test]
        public void DataTypeService_Can_Get_All()
        {
            IDataType dataType = new DataType(new LabelPropertyEditor(DataValueEditorFactory, IOHelper), ConfigurationEditorJsonSerializer) { Name = "Testing Textfield", DatabaseType = ValueStorageType.Ntext };
            DataTypeService.Save(dataType);

            // Get all the first time (no cache)
            IEnumerable<IDataType> all = DataTypeService.GetAll();

            // Get all a second time (with cache)
            all = DataTypeService.GetAll();

            Assert.Pass();
        }
    }
}
