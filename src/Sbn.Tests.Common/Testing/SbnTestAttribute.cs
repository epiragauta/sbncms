// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using Sbn.Cms.Core;

namespace Sbn.Cms.Tests.Common.Testing
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, /*AllowMultiple = false,*/ Inherited = false)]
    public class SbnTestAttribute : TestOptionAttributeBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether tests are "WithApplication".
        /// </summary>
        /// <remarks>
        /// <para>Default is false.</para>
        /// <para>This is for tests that inherited from TestWithApplicationBase.</para>
        /// <para>Implies Mapper = true (, ResetPluginManager = false).</para>
        /// </remarks>
        public bool WithApplication { get => _withApplication.ValueOrDefault(false); set => _withApplication.Set(value); }

        private readonly Settable<bool> _withApplication = new Settable<bool>();

        /// <summary>
        /// Gets or sets a value indicating whether to compose and initialize the mapper.
        /// </summary>
        /// <remarks>Default is false unless WithApplication is true, in which case default is true.</remarks>
        public bool Mapper { get => _mapper.ValueOrDefault(WithApplication); set => _mapper.Set(value); }

        private readonly Settable<bool> _mapper = new Settable<bool>();

        /// <summary>
        /// Gets or sets a value indicating whether the LEGACY XML Cache used in tests should bind to repository events
        /// </summary>
        public bool PublishedRepositoryEvents { get => _publishedRepositoryEvents.ValueOrDefault(false); set => _publishedRepositoryEvents.Set(value); }

        private readonly Settable<bool> _publishedRepositoryEvents = new Settable<bool>();

        /// <summary>
        /// Gets or sets a value indicating the required logging support.
        /// </summary>
        /// <remarks>Default is to mock logging.</remarks>
        public SbnTestOptions.Logger Logger { get => _logger.ValueOrDefault(SbnTestOptions.Logger.Mock); set => _logger.Set(value); }

        private readonly Settable<SbnTestOptions.Logger> _logger = new Settable<SbnTestOptions.Logger>();

        /// <summary>
        /// Gets or sets a value indicating the required database support.
        /// </summary>
        /// <remarks>Default is no database support.</remarks>
        public SbnTestOptions.Database Database { get => _database.ValueOrDefault(SbnTestOptions.Database.None); set => _database.Set(value); }

        private readonly Settable<SbnTestOptions.Database> _database = new Settable<SbnTestOptions.Database>();

        /// <summary>
        /// Gets or sets a value indicating the required plugin manager support.
        /// </summary>
        /// <remarks>Default is to use the global tests plugin manager.</remarks>
        public SbnTestOptions.TypeLoader TypeLoader { get => _typeLoader.ValueOrDefault(SbnTestOptions.TypeLoader.Default); set => _typeLoader.Set(value); }

        public bool Boot { get => _boot.ValueOrDefault(false); set => _boot.Set(value); }

        private readonly Settable<bool> _boot = new Settable<bool>();

        private readonly Settable<SbnTestOptions.TypeLoader> _typeLoader = new Settable<SbnTestOptions.TypeLoader>();

        protected override TestOptionAttributeBase Merge(TestOptionAttributeBase other)
        {
            if (!(other is SbnTestAttribute attr))
            {
                throw new ArgumentException(nameof(other));
            }

            base.Merge(other);
            _boot.Set(attr.Boot);
            _mapper.Set(attr._mapper);
            _publishedRepositoryEvents.Set(attr._publishedRepositoryEvents);
            _logger.Set(attr._logger);
            _database.Set(attr._database);
            _typeLoader.Set(attr._typeLoader);

            return this;
        }
    }
}
