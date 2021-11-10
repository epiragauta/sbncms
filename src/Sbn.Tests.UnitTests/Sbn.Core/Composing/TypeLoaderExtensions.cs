// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Collections.Generic;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Core.Composing
{
    /// <summary>
    /// Used for PluginTypeResolverTests
    /// </summary>
    internal static class TypeLoaderExtensions
    {
        public static IEnumerable<Type> ResolveFindMeTypes(this TypeLoader resolver)
        {
            return resolver.GetTypes<TypeLoaderTests.IFindMe>();
        }
    }
}
