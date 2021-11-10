// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.Common.Builders.Interfaces
{
    public interface IAccountBuilder : IWithLoginBuilder,
                                       IWithEmailBuilder,
                                       IWithFailedPasswordAttemptsBuilder,
                                       IWithIsApprovedBuilder,
                                       IWithIsLockedOutBuilder,
                                       IWithLastLoginDateBuilder,
                                       IWithLastPasswordChangeDateBuilder
    {
    }
}
