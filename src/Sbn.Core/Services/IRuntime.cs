using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Sbn.Cms.Core.Services
{
    /// <summary>
    /// Defines the Sbn runtime.
    /// </summary>
    public interface IRuntime : IHostedService
    {
        /// <summary>
        /// Gets the runtime state.
        /// </summary>
        IRuntimeState State { get; }

        /// <summary>
        /// Stops and Starts the runtime using the original cancellation token.
        /// </summary>
        Task RestartAsync();
    }
}
