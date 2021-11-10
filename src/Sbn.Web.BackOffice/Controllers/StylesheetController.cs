using System.Collections.Generic;
using System.Linq;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Services;
using Sbn.Cms.Web.Common.Attributes;
using Sbn.Extensions;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Web.BackOffice.Controllers
{
    /// <summary>
    /// The API controller used for retrieving available stylesheets
    /// </summary>
    [PluginController(Constants.Web.Mvc.BackOfficeApiArea)]
    public class StylesheetController : SbnAuthorizedJsonController
    {
        private readonly IFileService _fileService;

        public StylesheetController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IEnumerable<Stylesheet> GetAll()
        {
            return _fileService.GetStylesheets()
                .Select(x =>
                    new Stylesheet() {
                        Name = x.Alias,
                        Path = x.VirtualPath
                    });
        }

        public IEnumerable<StylesheetRule> GetRulesByName(string name)
        {
            var css = _fileService.GetStylesheet(name.EnsureEndsWith(".css"));
            if (css == null)
                return Enumerable.Empty<StylesheetRule>();

            return css.Properties.Select(x => new StylesheetRule() { Name = x.Name, Selector = x.Alias });
        }
    }

}
