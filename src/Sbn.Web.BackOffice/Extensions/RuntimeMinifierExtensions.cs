using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Manifest;
using Sbn.Cms.Core.WebAssets;
using Sbn.Cms.Infrastructure.WebAssets;

namespace Sbn.Extensions
{
    public static class RuntimeMinifierExtensions
    {
        /// <summary>
        /// Returns the JavaScript to load the back office's assets
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetScriptForLoadingBackOfficeAsync(
            this IRuntimeMinifier minifier,
            GlobalSettings globalSettings,
            IHostingEnvironment hostingEnvironment,
            IManifestParser manifestParser)
        {
            var files = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            foreach(var file in await minifier.GetJsAssetPathsAsync(BackOfficeWebAssets.SbnCoreJsBundleName))
            {
                files.Add(file);
            }

            foreach (var file in await minifier.GetJsAssetPathsAsync(BackOfficeWebAssets.SbnExtensionsJsBundleName))
            {
                files.Add(file);
            }

            // process the independent bundles
            if (manifestParser.CombinedManifest.Scripts.TryGetValue(BundleOptions.Independent, out IReadOnlyList<ManifestAssets> independentManifestAssetsList))
            {
                foreach (ManifestAssets manifestAssets in independentManifestAssetsList)
                {
                    var bundleName = BackOfficeWebAssets.GetIndependentPackageBundleName(manifestAssets, AssetType.Javascript);
                    foreach(var asset in await minifier.GetJsAssetPathsAsync(bundleName))
                    {
                        files.Add(asset);
                    }
                }
            }

            // process the "None" bundles, meaning we'll just render the script as-is
            foreach (var asset in await minifier.GetJsAssetPathsAsync(BackOfficeWebAssets.SbnNonOptimizedPackageJsBundleName))
            {
                files.Add(asset);
            }

            var result = BackOfficeJavaScriptInitializer.GetJavascriptInitialization(
                files,
                "sbn",
                globalSettings,
                hostingEnvironment);

            result += await GetStylesheetInitializationAsync(minifier, manifestParser);

            return result;
        }

        /// <summary>
        /// Gets the back office css bundle paths and formats a JS call to lazy load them
        /// </summary>
        private static async Task<string> GetStylesheetInitializationAsync(
            IRuntimeMinifier minifier,
            IManifestParser manifestParser)
        {
            var files = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            foreach(var file in await minifier.GetCssAssetPathsAsync(BackOfficeWebAssets.SbnCssBundleName))
            {
                files.Add(file);
            }

            // process the independent bundles
            if (manifestParser.CombinedManifest.Stylesheets.TryGetValue(BundleOptions.Independent, out IReadOnlyList<ManifestAssets> independentManifestAssetsList))
            {
                foreach (ManifestAssets manifestAssets in independentManifestAssetsList)
                {
                    var bundleName = BackOfficeWebAssets.GetIndependentPackageBundleName(manifestAssets, AssetType.Css);
                    foreach (var asset in await minifier.GetCssAssetPathsAsync(bundleName))
                    {
                        files.Add(asset);
                    }
                }
            }

            // process the "None" bundles, meaning we'll just render the script as-is
            foreach (var asset in await minifier.GetCssAssetPathsAsync(BackOfficeWebAssets.SbnNonOptimizedPackageCssBundleName))
            {
                files.Add(asset);
            }

            var sb = new StringBuilder();
            foreach (string file in files)
            {
                sb.AppendFormat("{0}LazyLoad.css('{1}');", Environment.NewLine, file);
            }

            return sb.ToString();
        }

    }
}
