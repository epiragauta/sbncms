using Microsoft.Extensions.Hosting;
using Sbn.Cms.Core.Hosting;

namespace Sbn.Cms.Web.Common.AspNetCore
{
    public class AspNetCoreSbnApplicationLifetime : ISbnApplicationLifetime
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public AspNetCoreSbnApplicationLifetime(IHostApplicationLifetime hostApplicationLifetime)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public bool IsRestarting { get; set; }

        public void Restart()
        {
            IsRestarting = true;
            _hostApplicationLifetime.StopApplication();
        }
    }
}
