using Microsoft.Extensions.Logging;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.Serialization;

namespace Sbn.Cms.Core.Configuration.Grid
{
    public class GridConfig : IGridConfig
    {
        public GridConfig(AppCaches appCaches, IManifestParser manifestParser, IJsonSerializer jsonSerializer, IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            EditorsConfig = new GridEditorsConfig(appCaches, hostingEnvironment, manifestParser, jsonSerializer, loggerFactory.CreateLogger<GridEditorsConfig>());
        }

        public IGridEditorsConfig EditorsConfig { get; }
    }
}
