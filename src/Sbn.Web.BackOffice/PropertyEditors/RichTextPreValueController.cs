using System.Linq;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Web.BackOffice.Controllers;
using Sbn.Cms.Web.Common.Attributes;

namespace Sbn.Cms.Web.BackOffice.PropertyEditors
{
    /// <summary>
    /// ApiController to provide RTE configuration with available plugins and commands from the RTE config
    /// </summary>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    public class RichTextPreValueController : SbnAuthorizedJsonController
    {
        private readonly IOptions<RichTextEditorSettings> _richTextEditorSettings;

        public RichTextPreValueController(IOptions<RichTextEditorSettings> richTextEditorSettings)
        {
            _richTextEditorSettings = richTextEditorSettings;
        }

        public RichTextEditorConfiguration GetConfiguration()
        {
            var settings = _richTextEditorSettings.Value;

            var config = new RichTextEditorConfiguration
            {
                Plugins = settings.Plugins.Select(x=>new RichTextEditorPlugin()
                {
                    Name = x
                }),
                Commands = settings.Commands.Select(x=>new RichTextEditorCommand()
                {
                    Alias = x.Alias,
                    Mode = x.Mode,
                    Name = x.Name
                }),
                ValidElements = settings.ValidElements,
                InvalidElements = settings.InvalidElements,
                CustomConfig = settings.CustomConfig
            };

            return config;
        }
    }
}
