using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.Commands;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Processors;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.DependencyInjection;
using Sbn.Cms.Web.Common.DependencyInjection;
using Sbn.Cms.Web.Common.ImageProcessors;

namespace Sbn.Extensions
{
    public static partial class SbnBuilderExtensions
    {
        /// <summary>
        /// Adds Image Sharp with Sbn settings
        /// </summary>
        public static IServiceCollection AddSbnImageSharp(this ISbnBuilder builder)
        {
            ImagingSettings imagingSettings = builder.Config.GetSection(Cms.Core.Constants.Configuration.ConfigImaging)
                .Get<ImagingSettings>() ?? new ImagingSettings();

            builder.Services.AddImageSharp(options =>
            {
                // The configuration is set using ImageSharpConfigurationOptions
                options.BrowserMaxAge = imagingSettings.Cache.BrowserMaxAge;
                options.CacheMaxAge = imagingSettings.Cache.CacheMaxAge;
                options.CachedNameLength = imagingSettings.Cache.CachedNameLength;

                // Use configurable maximum width and height (overwrite ImageSharps default)
                options.OnParseCommandsAsync = context =>
                {
                    if (context.Commands.Count == 0)
                    {
                        return Task.CompletedTask;
                    }

                    uint width = context.Parser.ParseValue<uint>(context.Commands.GetValueOrDefault(ResizeWebProcessor.Width), context.Culture);
                    uint height = context.Parser.ParseValue<uint>(context.Commands.GetValueOrDefault(ResizeWebProcessor.Height), context.Culture);
                    if (width > imagingSettings.Resize.MaxWidth || height > imagingSettings.Resize.MaxHeight)
                    {
                        context.Commands.Remove(ResizeWebProcessor.Width);
                        context.Commands.Remove(ResizeWebProcessor.Height);
                    }

                    return Task.CompletedTask;
                };
            })
                .Configure<PhysicalFileSystemCacheOptions>(options => options.CacheFolder = builder.BuilderHostingEnvironment.MapPathContentRoot(imagingSettings.Cache.CacheFolder))
                // We need to add CropWebProcessor before ResizeWebProcessor (until https://github.com/SixLabors/ImageSharp.Web/issues/182 is fixed)
                .RemoveProcessor<ResizeWebProcessor>()
                .RemoveProcessor<FormatWebProcessor>()
                .RemoveProcessor<BackgroundColorWebProcessor>()
                .RemoveProcessor<JpegQualityWebProcessor>()
                .AddProcessor<CropWebProcessor>()
                .AddProcessor<ResizeWebProcessor>()
                .AddProcessor<FormatWebProcessor>()
                .AddProcessor<BackgroundColorWebProcessor>()
                .AddProcessor<JpegQualityWebProcessor>();

            builder.Services.AddTransient<IConfigureOptions<ImageSharpMiddlewareOptions>, ImageSharpConfigurationOptions>();

            return builder.Services;
        }
    }
}
