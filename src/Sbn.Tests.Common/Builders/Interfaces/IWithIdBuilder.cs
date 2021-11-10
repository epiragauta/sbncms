// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithIdBuilder
    {
        int? Id { get; set; }
    }

    public interface IWithIdBuilder<TId>
    {
        TId Id { get; set; }
    }
}
