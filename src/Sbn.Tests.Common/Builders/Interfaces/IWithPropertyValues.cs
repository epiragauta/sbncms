// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithPropertyValues
    {
        object PropertyValues { get; set; }

        string PropertyValuesCulture { get; set; }

        string PropertyValuesSegment { get; set; }
    }
}
