using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Core.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/>
    /// </summary>
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds Sbn composers for plugins
        /// </summary>
        public static ISbnBuilder AddComposers(this ISbnBuilder builder)
        {
            IEnumerable<Type> composerTypes = builder.TypeLoader.GetTypes<IComposer>();
            IEnumerable<Attribute> enableDisable = builder.TypeLoader.GetAssemblyAttributes(typeof(EnableComposerAttribute), typeof(DisableComposerAttribute));

            new ComposerGraph(builder, composerTypes, enableDisable, builder.BuilderLoggerFactory.CreateLogger<ComposerGraph>()).Compose();

            return builder;
        }
    }
}
