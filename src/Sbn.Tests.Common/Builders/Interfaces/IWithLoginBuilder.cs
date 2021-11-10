// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IWithLoginBuilder
    {
        string Username { get; set; }

        string RawPasswordValue { get; set; }

        string PasswordConfig { get; set; }
    }
}
