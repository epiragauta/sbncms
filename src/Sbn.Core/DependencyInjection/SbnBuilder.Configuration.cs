using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Configuration.Models.Validation;

namespace Sbn.Cms.Core.DependencyInjection
{
    /// <summary>
    /// Extension methods for <see cref="ISbnBuilder"/>
    /// </summary>
    public static partial class SbnBuilderExtensions
    {

        private static ISbnBuilder AddSbnOptions<TOptions>(this ISbnBuilder builder)
            where TOptions : class
        {
            var sbnOptionsAttribute = typeof(TOptions).GetCustomAttribute<SbnOptionsAttribute>();

            if (sbnOptionsAttribute is null)
            {
                throw new ArgumentException("typeof(TOptions) do not have the SbnOptionsAttribute");
            }


            builder.Services.AddOptions<TOptions>()
                .Bind(builder.Config.GetSection(sbnOptionsAttribute.ConfigurationKey),
                    o => o.BindNonPublicProperties = sbnOptionsAttribute.BindNonPublicProperties)
                .ValidateDataAnnotations();

             return builder;
        }

        /// <summary>
        /// Add Sbn configuration services and options
        /// </summary>
        public static ISbnBuilder AddConfiguration(this ISbnBuilder builder)
        {
            // Register configuration validators.
            builder.Services.AddSingleton<IValidateOptions<ContentSettings>, ContentSettingsValidator>();
            builder.Services.AddSingleton<IValidateOptions<GlobalSettings>, GlobalSettingsValidator>();
            builder.Services.AddSingleton<IValidateOptions<HealthChecksSettings>, HealthChecksSettingsValidator>();
            builder.Services.AddSingleton<IValidateOptions<RequestHandlerSettings>, RequestHandlerSettingsValidator>();
            builder.Services.AddSingleton<IValidateOptions<UnattendedSettings>, UnattendedSettingsValidator>();

            // Register configuration sections.
            builder
                .AddSbnOptions<ModelsBuilderSettings>()
                .AddSbnOptions<ConnectionStrings>()
                .AddSbnOptions<ActiveDirectorySettings>()
                .AddSbnOptions<ContentSettings>()
                .AddSbnOptions<CoreDebugSettings>()
                .AddSbnOptions<ExceptionFilterSettings>()
                .AddSbnOptions<GlobalSettings>()
                .AddSbnOptions<HealthChecksSettings>()
                .AddSbnOptions<HostingSettings>()
                .AddSbnOptions<ImagingSettings>()
                .AddSbnOptions<IndexCreatorSettings>()
                .AddSbnOptions<KeepAliveSettings>()
                .AddSbnOptions<LoggingSettings>()
                .AddSbnOptions<MemberPasswordConfigurationSettings>()
                .AddSbnOptions<NuCacheSettings>()
                .AddSbnOptions<RequestHandlerSettings>()
                .AddSbnOptions<RuntimeSettings>()
                .AddSbnOptions<SecuritySettings>()
                .AddSbnOptions<TourSettings>()
                .AddSbnOptions<TypeFinderSettings>()
                .AddSbnOptions<UserPasswordConfigurationSettings>()
                .AddSbnOptions<WebRoutingSettings>()
                .AddSbnOptions<SbnPluginSettings>()
                .AddSbnOptions<UnattendedSettings>()
                .AddSbnOptions<RichTextEditorSettings>()
                .AddSbnOptions<BasicAuthSettings>()
                .AddSbnOptions<RuntimeMinificationSettings>()
                .AddSbnOptions<PackageMigrationSettings>();

            return builder;
        }
    }
}
