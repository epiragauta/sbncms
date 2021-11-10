// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.Globalization;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using Sbn.Cms.Core.Macros;
using Sbn.Cms.Core.Models.PublishedContent;
using Sbn.Cms.Core.Strings;
using Sbn.Cms.Core.Templates;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Infrastructure.Macros;
using Sbn.Extensions;

namespace Sbn.Cms.Core.PropertyEditors.ValueConverters
{
    /// <summary>
    /// A value converter for TinyMCE that will ensure any macro content is rendered properly even when
    /// used dynamically.
    /// </summary>
    [DefaultPropertyValueConverter]
    public class RteMacroRenderingValueConverter : SimpleTinyMceValueConverter
    {
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IMacroRenderer _macroRenderer;
        private readonly HtmlLocalLinkParser _linkParser;
        private readonly HtmlUrlParser _urlParser;
        private readonly HtmlImageSourceParser _imageSourceParser;

        public override PropertyCacheLevel GetPropertyCacheLevel(IPublishedPropertyType propertyType)
        {
            // because that version of RTE converter parses {locallink} and executes macros, its value has
            // to be cached at the published snapshot level, because we have no idea what the macros may depend on actually.
            return PropertyCacheLevel.Snapshot;
        }

        public RteMacroRenderingValueConverter(ISbnContextAccessor sbnContextAccessor, IMacroRenderer macroRenderer,
            HtmlLocalLinkParser linkParser, HtmlUrlParser urlParser, HtmlImageSourceParser imageSourceParser)
        {
            _sbnContextAccessor = sbnContextAccessor;
            _macroRenderer = macroRenderer;
            _linkParser = linkParser;
            _urlParser = urlParser;
            _imageSourceParser = imageSourceParser;
        }

        // NOT thread-safe over a request because it modifies the
        // global SbnContext.Current.InPreviewMode status. So it
        // should never execute in // over the same SbnContext with
        // different preview modes.
        private string RenderRteMacros(string source, bool preview)
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            using (sbnContext.ForcedPreview(preview)) // force for macro rendering
            {
                var sb = new StringBuilder();

                MacroTagParser.ParseMacros(
                    source,
                    //callback for when text block is found
                    textBlock => sb.Append(textBlock),
                    //callback for when macro syntax is found
                    (macroAlias, macroAttributes) => sb.Append(_macroRenderer.RenderAsync(
                        macroAlias,
                        sbnContext.PublishedRequest?.PublishedContent,
                        //needs to be explicitly casted to Dictionary<string, object>
                        macroAttributes.ConvertTo(x => (string)x, x => x)).GetAwaiter().GetResult().Text));

                return sb.ToString();
            }
        }

        public override object ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object inter, bool preview)
        {
            var converted = Convert(inter, preview);

            return new HtmlEncodedString(converted == null ? string.Empty : converted);
        }

        private string Convert(object source, bool preview)
        {
            if (source == null)
            {
                return null;
            }

            var sourceString = source.ToString();

            // ensures string is parsed for {localLink} and URLs and media are resolved correctly
            sourceString = _linkParser.EnsureInternalLinks(sourceString, preview);
            sourceString = _urlParser.EnsureUrls(sourceString);
            sourceString = _imageSourceParser.EnsureImageSources(sourceString);

            // ensure string is parsed for macros and macros are executed correctly
            sourceString = RenderRteMacros(sourceString, preview);

            // find and remove the rel attributes used in the Sbn UI from img tags
            var doc = new HtmlDocument();
            doc.LoadHtml(sourceString);

            if (doc.ParseErrors.Any() == false && doc.DocumentNode != null)
            {
                // Find all images with rel attribute
                var imgNodes = doc.DocumentNode.SelectNodes("//img[@rel]");

                var modified = false;
                if (imgNodes != null)
                {
                    foreach (var img in imgNodes)
                    {
                        var nodeId = img.GetAttributeValue("rel", string.Empty);
                        if (int.TryParse(nodeId, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
                        {
                            img.Attributes.Remove("rel");
                            modified = true;
                        }
                    }
                }

                // Find all a and img tags with a data-udi attribute
                var dataUdiNodes = doc.DocumentNode.SelectNodes("(//a|//img)[@data-udi]");
                if (dataUdiNodes != null)
                {
                    foreach (var node in dataUdiNodes)
                    {
                        node.Attributes.Remove("data-udi");
                        modified = true;
                    }
                }

                if (modified)
                {
                    return doc.DocumentNode.OuterHtml;
                }
            }

            return sourceString;
        }
    }
}
