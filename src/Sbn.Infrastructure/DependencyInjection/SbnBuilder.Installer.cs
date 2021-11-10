using Microsoft.Extensions.DependencyInjection;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Core.Install.InstallSteps;
using Sbn.Cms.Core.Install.Models;
using Sbn.Cms.Infrastructure.Install;
using Sbn.Cms.Infrastructure.Install.InstallSteps;
using Sbn.Extensions;

namespace Sbn.Cms.Infrastructure.DependencyInjection
{
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds the services for the Sbn installer
        /// </summary>
        internal static ISbnBuilder AddInstaller(this ISbnBuilder builder)
        {
            // register the installer steps
            builder.Services.AddScoped<InstallSetupStep, NewInstallStep>();
            builder.Services.AddScoped<InstallSetupStep, UpgradeStep>();
            builder.Services.AddScoped<InstallSetupStep, FilePermissionsStep>();
            builder.Services.AddScoped<InstallSetupStep, TelemetryIdentifierStep>();
            builder.Services.AddScoped<InstallSetupStep, DatabaseConfigureStep>();
            builder.Services.AddScoped<InstallSetupStep, DatabaseInstallStep>();
            builder.Services.AddScoped<InstallSetupStep, DatabaseUpgradeStep>();

            builder.Services.AddScoped<InstallSetupStep, CompleteInstallStep>();

            builder.Services.AddTransient<InstallStepCollection>();
            builder.Services.AddUnique<InstallHelper>();

            builder.Services.AddTransient<PackageMigrationRunner>();

            return builder;
        }
    }
}
