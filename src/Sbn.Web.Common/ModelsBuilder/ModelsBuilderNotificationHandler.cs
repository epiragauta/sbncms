using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Core.Notifications;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Infrastructure.ModelsBuilder;
using Sbn.Cms.Infrastructure.WebAssets;
using Sbn.Cms.Web.Common.ModelBinders;

namespace Sbn.Cms.Web.Common.ModelsBuilder
{
    /// <summary>
    /// Handles <see cref="SbnApplicationStartingNotification"/> and <see cref="ServerVariablesParsingNotification"/> notifications to initialize MB
    /// </summary>
    internal class ModelsBuilderNotificationHandler :
        INotificationHandler<ServerVariablesParsingNotification>,
        INotificationHandler<ModelBindingErrorNotification>,
        INotificationHandler<TemplateSavingNotification>
    {
        private readonly ModelsBuilderSettings _config;
        private readonly IShortStringHelper _shortStringHelper;
        private readonly IModelsBuilderDashboardProvider _modelsBuilderDashboardProvider;

        public ModelsBuilderNotificationHandler(
            IOptions<ModelsBuilderSettings> config,
            IShortStringHelper shortStringHelper,
            IModelsBuilderDashboardProvider modelsBuilderDashboardProvider)
        {
            _config = config.Value;
            _shortStringHelper = shortStringHelper;
            _modelsBuilderDashboardProvider = modelsBuilderDashboardProvider;
        }

        /// <summary>
        /// Handles the <see cref="ServerVariablesParsingNotification"/> notification to add custom urls and MB mode
        /// </summary>
        public void Handle(ServerVariablesParsingNotification notification)
        {
            IDictionary<string, object> serverVars = notification.ServerVariables;

            if (!serverVars.ContainsKey("sbnUrls"))
            {
                throw new ArgumentException("Missing sbnUrls.");
            }

            var sbnUrlsObject = serverVars["sbnUrls"];
            if (sbnUrlsObject == null)
            {
                throw new ArgumentException("Null sbnUrls");
            }

            if (!(sbnUrlsObject is Dictionary<string, object> sbnUrls))
            {
                throw new ArgumentException("Invalid sbnUrls");
            }

            if (!serverVars.ContainsKey("sbnPlugins"))
            {
                throw new ArgumentException("Missing sbnPlugins.");
            }

            if (!(serverVars["sbnPlugins"] is Dictionary<string, object> sbnPlugins))
            {
                throw new ArgumentException("Invalid sbnPlugins");
            }

            sbnUrls["modelsBuilderBaseUrl"] = _modelsBuilderDashboardProvider.GetUrl();
            sbnPlugins["modelsBuilder"] = GetModelsBuilderSettings();
        }

        private Dictionary<string, object> GetModelsBuilderSettings()
        {
            var settings = new Dictionary<string, object>
            {
                {"mode", _config.ModelsMode.ToString() }
            };

            return settings;
        }

        /// <summary>
        /// Used to check if a template is being created based on a document type, in this case we need to
        /// ensure the template markup is correct based on the model name of the document type
        /// </summary>
        public void Handle(TemplateSavingNotification notification)
        {
            if (_config.ModelsMode == ModelsMode.Nothing)
            {
                return;
            }

            // Don't do anything if we're not requested to create a template for a content type
            if (notification.CreateTemplateForContentType is false)
            {
                return;
            }

            // ensure we have the content type alias
            if (notification.ContentTypeAlias is null)
            {
                throw new InvalidOperationException("ContentTypeAlias was not found on the notification");
            }

            foreach (ITemplate template in notification.SavedEntities)
            {
                // if it is in fact a new entity (not been saved yet) and the "CreateTemplateForContentType" key
                // is found, then it means a new template is being created based on the creation of a document type
                if (!template.HasIdentity && string.IsNullOrWhiteSpace(template.Content))
                {
                    // ensure is safe and always pascal cased, per razor standard
                    // + this is how we get the default model name in Sbn.ModelsBuilder.Sbn.Application
                    var alias = notification.ContentTypeAlias;
                    var name = template.Name; // will be the name of the content type since we are creating
                    var className = SbnServices.GetClrName(_shortStringHelper, name, alias);

                    var modelNamespace = _config.ModelsNamespace;

                    // we do not support configuring this at the moment, so just let Sbn use its default value
                    // var modelNamespaceAlias = ...;
                    var markup = ViewHelper.GetDefaultFileContent(
                        modelClassName: className,
                        modelNamespace: modelNamespace/*,
                        modelNamespaceAlias: modelNamespaceAlias*/);

                    // set the template content to the new markup
                    template.Content = markup;
                }
            }
        }

        /// <summary>
        /// Handles when a model binding error occurs
        /// </summary>
        public void Handle(ModelBindingErrorNotification notification)
        {
            ModelsBuilderAssemblyAttribute sourceAttr = notification.SourceType.Assembly.GetCustomAttribute<ModelsBuilderAssemblyAttribute>();
            ModelsBuilderAssemblyAttribute modelAttr = notification.ModelType.Assembly.GetCustomAttribute<ModelsBuilderAssemblyAttribute>();

            // if source or model is not a ModelsBuider type...
            if (sourceAttr == null || modelAttr == null)
            {
                // if neither are ModelsBuilder types, give up entirely
                if (sourceAttr == null && modelAttr == null)
                {
                    return;
                }

                // else report, but better not restart (loops?)
                notification.Message.Append(" The ");
                notification.Message.Append(sourceAttr == null ? "view model" : "source");
                notification.Message.Append(" is a ModelsBuilder type, but the ");
                notification.Message.Append(sourceAttr != null ? "view model" : "source");
                notification.Message.Append(" is not. The application is in an unstable state and should be restarted.");
                return;
            }

            // both are ModelsBuilder types
            var pureSource = sourceAttr.IsInMemory;
            var pureModel = modelAttr.IsInMemory;

            if (sourceAttr.IsInMemory || modelAttr.IsInMemory)
            {
                if (pureSource == false || pureModel == false)
                {
                    // only one is pure - report, but better not restart (loops?)
                    notification.Message.Append(pureSource
                        ? " The content model is in memory generated, but the view model is not."
                        : " The view model is in memory generated, but the content model is not.");
                    notification.Message.Append(" The application is in an unstable state and should be restarted.");
                }
                else
                {
                    // both are pure - report, and if different versions, restart
                    // if same version... makes no sense... and better not restart (loops?)
                    Version sourceVersion = notification.SourceType.Assembly.GetName().Version;
                    Version modelVersion = notification.ModelType.Assembly.GetName().Version;
                    notification.Message.Append(" Both view and content models are in memory generated, with ");
                    notification.Message.Append(sourceVersion == modelVersion
                        ? "same version. The application is in an unstable state and should be restarted."
                        : "different versions. The application is in an unstable state and should be restarted.");
                }
            }
        }
    }
}
