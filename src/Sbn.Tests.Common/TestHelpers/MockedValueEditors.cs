// Copyright (c) Sbn.
// See LICENSE for more details.

using Moq;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.PropertyEditors;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.Serialization;

namespace Sbn.Cms.Tests.Common.TestHelpers
{
    public class MockedValueEditors
    {
        public static DataValueEditor CreateDataValueEditor(string name)
        {
            var valueType = ValueTypes.IsValue(name) ? name : ValueTypes.String;

            return new DataValueEditor(
                Mock.Of<ILocalizedTextService>(),
                Mock.Of<IShortStringHelper>(),
                new JsonNetSerializer(),
                Mock.Of<IIOHelper>(),
                new DataEditorAttribute(name, name, name)
                {
                    ValueType = valueType
                });
        }
    }
}
