using System.IO;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Cache;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Notifications;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.ModelsBuilder
{
    /// <summary>
    /// Used to track if ModelsBuilder models are out of date/stale
    /// </summary>
    public sealed class OutOfDateModelsStatus : INotificationHandler<ContentTypeCacheRefresherNotification>,
        INotificationHandler<DataTypeCacheRefresherNotification>
    {
        private readonly ModelsBuilderSettings _config;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfDateModelsStatus"/> class.
        /// </summary>
        public OutOfDateModelsStatus(IOptions<ModelsBuilderSettings> config, IHostingEnvironment hostingEnvironment)
        {
            _config = config.Value;
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// Gets a value indicating whether flagging out of date models is enabled
        /// </summary>
        public bool IsEnabled => _config.FlagOutOfDateModels;

        /// <summary>
        /// Gets a value indicating whether models are out of date
        /// </summary>
        public bool IsOutOfDate
        {
            get
            {
                if (_config.FlagOutOfDateModels == false)
                {
                    return false;
                }

                var path = GetFlagPath();
                return path != null && File.Exists(path);
            }
        }


        private string GetFlagPath()
        {
            var modelsDirectory = _config.ModelsDirectoryAbsolute(_hostingEnvironment);
            if (!Directory.Exists(modelsDirectory))
            {
                Directory.CreateDirectory(modelsDirectory);
            }

            return Path.Combine(modelsDirectory, "ood.flag");
        }

        private void Write()
        {
            // don't run if not configured
            if (!IsEnabled)
            {
                return;
            }

            var path = GetFlagPath();
            if (path == null || File.Exists(path))
            {
                return;
            }

            File.WriteAllText(path, "THIS FILE INDICATES THAT MODELS ARE OUT-OF-DATE\n\n");
        }

        public void Clear()
        {
            if (_config.FlagOutOfDateModels == false)
            {
                return;
            }

            var path = GetFlagPath();
            if (path == null || !File.Exists(path))
            {
                return;
            }

            File.Delete(path);
        }

        public void Handle(ContentTypeCacheRefresherNotification notification) => Write();

        public void Handle(DataTypeCacheRefresherNotification notification) => Write();
    }
}
