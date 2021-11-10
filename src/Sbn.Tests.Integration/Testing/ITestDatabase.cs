// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Tests.Integration.Testing
{
    public interface ITestDatabase
    {
        TestDbMeta AttachEmpty();

        TestDbMeta AttachSchema();

        void Detach(TestDbMeta id);
    }
}
