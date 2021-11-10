// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.ComponentModel;

namespace Sbn.Cms.Core.Configuration.Models
{
    /// <summary>
    /// Typed configuration options for global settings.
    /// </summary>
    [SbnOptions(Constants.Configuration.ConfigGlobal)]
    public class GlobalSettings
    {
        internal const string StaticReservedPaths = "~/app_plugins/,~/install/,~/mini-profiler-resources/,~/sbn/,"; // must end with a comma!
        internal const string StaticReservedUrls = "~/.well-known,"; // must end with a comma!
        internal const string StaticTimeOut = "00:20:00";
        internal const string StaticDefaultUILanguage = "en-US";
        internal const bool StaticHideTopLevelNodeFromPath = true;
        internal const bool StaticUseHttps = false;
        internal const int StaticVersionCheckPeriod = 7;
        internal const string StaticSbnPath = "~/sbn";
        internal const string StaticIconsPath = "~/sbn/assets/icons";
        internal const string StaticSbnCssPath = "~/css";
        internal const string StaticSbnScriptsPath = "~/scripts";
        internal const string StaticSbnMediaPath = "~/media";
        internal const bool StaticInstallMissingDatabase = false;
        internal const bool StaticDisableElectionForSingleServer = false;
        internal const string StaticNoNodesViewPath = "~/sbn/SbnWebsite/NoNodes.cshtml";
        internal const string StaticSqlWriteLockTimeOut = "00:00:05";

        /// <summary>
        /// Gets or sets a value for the reserved URLs.
        /// It must end with a comma
        /// </summary>
        [DefaultValue(StaticReservedUrls)]
        public string ReservedUrls { get; set; } = StaticReservedUrls;

        /// <summary>
        /// Gets or sets a value for the reserved paths.
        /// It must end with a comma
        /// </summary>
        [DefaultValue(StaticReservedPaths)]
        public string ReservedPaths { get; set; } = StaticReservedPaths;

        /// <summary>
        /// Gets or sets a value for the timeout
        /// </summary>
        [DefaultValue(StaticTimeOut)]
        public TimeSpan TimeOut { get; set; } = TimeSpan.Parse(StaticTimeOut);

        /// <summary>
        /// Gets or sets a value for the default UI language.
        /// </summary>
        [DefaultValue(StaticDefaultUILanguage)]
        public string DefaultUILanguage { get; set; } = StaticDefaultUILanguage;

        /// <summary>
        /// Gets or sets a value indicating whether to hide the top level node from the path.
        /// </summary>
        [DefaultValue(StaticHideTopLevelNodeFromPath)]
        public bool HideTopLevelNodeFromPath { get; set; } = StaticHideTopLevelNodeFromPath;

        /// <summary>
        /// Gets or sets a value indicating whether HTTPS should be used.
        /// </summary>
        [DefaultValue(StaticUseHttps)]
        public bool UseHttps { get; set; } = StaticUseHttps;

        /// <summary>
        /// Gets or sets a value for the version check period in days.
        /// </summary>
        [DefaultValue(StaticVersionCheckPeriod)]
        public int VersionCheckPeriod { get; set; } = StaticVersionCheckPeriod;

        /// <summary>
        /// Gets or sets a value for the Sbn back-office path.
        /// </summary>
        [DefaultValue(StaticSbnPath)]
        public string SbnPath { get; set; } = StaticSbnPath;

        /// <summary>
        /// Gets or sets a value for the Sbn icons path.
        /// </summary>
        /// <remarks>
        /// TODO: Sbn cannot be hard coded here that is what SbnPath is for
        ///       so this should not be a normal get set it has to have dynamic ability to return the correct
        ///       path given SbnPath if this hasn't been explicitly set.
        /// </remarks>
        [DefaultValue(StaticIconsPath)]
        public string IconsPath { get; set; } = StaticIconsPath;

        /// <summary>
        /// Gets or sets a value for the Sbn CSS path.
        /// </summary>
        [DefaultValue(StaticSbnCssPath)]
        public string SbnCssPath { get; set; } = StaticSbnCssPath;

        /// <summary>
        /// Gets or sets a value for the Sbn scripts path.
        /// </summary>
        [DefaultValue(StaticSbnScriptsPath)]
        public string SbnScriptsPath { get; set; } = StaticSbnScriptsPath;

        /// <summary>
        /// Gets or sets a value for the Sbn media path.
        /// </summary>
        [DefaultValue(StaticSbnMediaPath)]
        public string SbnMediaPath { get; set; } = StaticSbnMediaPath;

        /// <summary>
        /// Gets or sets a value indicating whether to install the database when it is missing.
        /// </summary>
        [DefaultValue(StaticInstallMissingDatabase)]
        public bool InstallMissingDatabase { get; set; } = StaticInstallMissingDatabase;

        /// <summary>
        /// Gets or sets a value indicating whether to disable the election for a single server.
        /// </summary>
        [DefaultValue(StaticDisableElectionForSingleServer)]
        public bool DisableElectionForSingleServer { get; set; } = StaticDisableElectionForSingleServer;

        /// <summary>
        /// Gets or sets a value for the database factory server version.
        /// </summary>
        public string DatabaseFactoryServerVersion { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value for the main dom lock.
        /// </summary>
        public string MainDomLock { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value for the path to the no content view.
        /// </summary>
        [DefaultValue(StaticNoNodesViewPath)]
        public string NoNodesViewPath { get; set; } = StaticNoNodesViewPath;

        /// <summary>
        /// Gets or sets a value for the database server registrar settings.
        /// </summary>
        public DatabaseServerRegistrarSettings DatabaseServerRegistrar { get; set; } = new DatabaseServerRegistrarSettings();

        /// <summary>
        /// Gets or sets a value for the database server messenger settings.
        /// </summary>
        public DatabaseServerMessengerSettings DatabaseServerMessenger { get; set; } = new DatabaseServerMessengerSettings();

        /// <summary>
        /// Gets or sets a value for the SMTP settings.
        /// </summary>
        public SmtpSettings Smtp { get; set; }

        /// <summary>
        /// Gets a value indicating whether SMTP is configured.
        /// </summary>
        public bool IsSmtpServerConfigured => !string.IsNullOrWhiteSpace(Smtp?.Host);

        /// <summary>
        /// An int value representing the time in milliseconds to lock the database for a write operation
        /// </summary>
        /// <remarks>
        /// The default value is 5000 milliseconds
        /// </remarks>
        /// <value>The timeout in milliseconds.</value>
        [DefaultValue(StaticSqlWriteLockTimeOut)]
        public TimeSpan SqlWriteLockTimeOut { get; } = TimeSpan.Parse(StaticSqlWriteLockTimeOut);
    }
}
