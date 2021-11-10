// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Lucene.Net.Index;
using NUnit.Framework;
using Sbn.Cms.Core;
using Sbn.Extensions;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core
{
    [TestFixture]
    public class DelegateExtensionsTests
    {
        [Test]
        public void Only_Executes_Specific_Count()
        {
            const int maxTries = 5;
            var totalTries = 0;
            DelegateExtensions.RetryUntilSuccessOrMaxAttempts(
                (currentTry) =>
                {
                    totalTries = currentTry;
                    return Attempt<IndexWriter>.Fail();
                },
                5,
                TimeSpan.FromMilliseconds(10));

            Assert.AreEqual(maxTries, totalTries);
        }

        [Test]
        public void Quits_On_Success_Count()
        {
            var totalTries = 0;
            DelegateExtensions.RetryUntilSuccessOrMaxAttempts(
                (currentTry) =>
                {
                    totalTries = currentTry;
                    return totalTries == 2 ? Attempt<string>.Succeed() : Attempt<string>.Fail();
                },
                5,
                TimeSpan.FromMilliseconds(10));

            Assert.AreEqual(2, totalTries);
        }
    }
}
