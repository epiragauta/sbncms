using Sbn.Cms.Core.DependencyInjection;

namespace Sbn.Cms.Core.Composing
{
    /// <summary>
    /// Represents a composer.
    /// </summary>
    public interface IComposer : IDiscoverable
    {
        /// <summary>
        /// Compose.
        /// </summary>
        void Compose(ISbnBuilder builder);
    }
}
