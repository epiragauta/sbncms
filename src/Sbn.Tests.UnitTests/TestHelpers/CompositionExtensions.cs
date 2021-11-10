// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;

namespace Sbn.Cms.Tests.UnitTests.TestHelpers
{
    public static class CompositionExtensions
    {
        [Obsolete("This extension method exists only to ease migration, please refactor")]
        public static IServiceProvider CreateServiceProvider(this ISbnBuilder builder)
        {
            builder.Build();
            return builder.Services.BuildServiceProvider();
        }
    }
}
