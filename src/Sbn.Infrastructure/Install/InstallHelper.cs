using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Install.Models;
using Sbn.Cms.Core.Net;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Infrastructure.Install
{
    public sealed class InstallHelper
    {
        private readonly DatabaseBuilder _databaseBuilder;
        private readonly ILogger<InstallHelper> _logger;
        private readonly ISbnVersion _sbnVersion;
        private readonly IOptionsMonitor<ConnectionStrings> _connectionStrings;
        private readonly IInstallationService _installationService;
        private readonly ICookieManager _cookieManager;
        private readonly IUserAgentProvider _userAgentProvider;
        private readonly ISbnDatabaseFactory _sbnDatabaseFactory;
        private InstallationType? _installationType;

        public InstallHelper(DatabaseBuilder databaseBuilder,
            ILogger<InstallHelper> logger,
            ISbnVersion sbnVersion,
            IOptionsMonitor<ConnectionStrings> connectionStrings,
            IInstallationService installationService,
            ICookieManager cookieManager,
            IUserAgentProvider userAgentProvider,
            ISbnDatabaseFactory sbnDatabaseFactory)
        {
            _logger = logger;
            _sbnVersion = sbnVersion;
            _databaseBuilder = databaseBuilder;
            _connectionStrings = connectionStrings;
            _installationService = installationService;
            _cookieManager = cookieManager;
            _userAgentProvider = userAgentProvider;
            _sbnDatabaseFactory = sbnDatabaseFactory;

            // We need to initialize the type already, as we can't detect later, if the connection string is added on the fly.
            GetInstallationType();
        }

        public InstallationType GetInstallationType() => _installationType ??= IsBrandNewInstall ? InstallationType.NewInstall : InstallationType.Upgrade;

        public async Task SetInstallStatusAsync(bool isCompleted, string errorMsg)
        {
            try
            {
                var userAgent = _userAgentProvider.GetUserAgent();

                // Check for current install ID
                var installCookie = _cookieManager.GetCookieValue(Constants.Web.InstallerCookieName);
                if (!Guid.TryParse(installCookie, out var installId))
                {
                    installId = Guid.NewGuid();

                    _cookieManager.SetCookieValue(Constants.Web.InstallerCookieName, installId.ToString());
                }

                var dbProvider = string.Empty;
                if (IsBrandNewInstall == false)
                {
                    // we don't have DatabaseProvider anymore... doing it differently
                    //dbProvider = ApplicationContext.Current.DatabaseContext.DatabaseProvider.ToString();
                    dbProvider = _sbnDatabaseFactory.SqlContext.SqlSyntax.DbProvider;
                }

                var installLog = new InstallLog(installId: installId, isUpgrade: IsBrandNewInstall == false,
                    installCompleted: isCompleted, timestamp: DateTime.Now, versionMajor: _sbnVersion.Version.Major,
                    versionMinor: _sbnVersion.Version.Minor, versionPatch: _sbnVersion.Version.Build,
                    versionComment: _sbnVersion.Comment, error: errorMsg, userAgent: userAgent,
                    dbProvider: dbProvider);

                await _installationService.LogInstall(installLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in InstallStatus trying to check upgrades");
            }
        }

        /// <summary>
        /// Checks if this is a brand new install, meaning that there is no configured database connection or the database is empty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this is a brand new install; otherwise, <c>false</c>.
        /// </value>
        private bool IsBrandNewInstall => _connectionStrings.CurrentValue.SbnConnectionString?.IsConnectionStringConfigured() != true ||
                    _databaseBuilder.IsDatabaseConfigured == false ||
                    _databaseBuilder.CanConnectToDatabase == false ||
                    _databaseBuilder.IsSbnInstalled() == false;
    }
}
