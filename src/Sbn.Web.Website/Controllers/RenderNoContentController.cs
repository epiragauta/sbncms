using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Sbn.Cms.Core.Configuration.Models;
using Sbn.Cms.Core.IO;
using Sbn.Cms.Core.Web;
using Sbn.Cms.Web.Website.Models;
using Sbn.Extensions;

namespace Sbn.Cms.Web.Website.Controllers
{
    public class RenderNoContentController : Controller
    {
        private readonly ISbnContextAccessor _sbnContextAccessor;
        private readonly IIOHelper _ioHelper;
        private readonly IOptions<GlobalSettings> _globalSettings;

        public RenderNoContentController(ISbnContextAccessor sbnContextAccessor, IIOHelper ioHelper, IOptions<GlobalSettings> globalSettings)
        {
            _sbnContextAccessor = sbnContextAccessor ?? throw new ArgumentNullException(nameof(sbnContextAccessor));
            _ioHelper = ioHelper ?? throw new ArgumentNullException(nameof(ioHelper));
            _globalSettings = globalSettings ?? throw new ArgumentNullException(nameof(globalSettings));
        }

        public ActionResult Index()
        {
            var sbnContext = _sbnContextAccessor.GetRequiredSbnContext();
            var store = sbnContext.Content;
            if (store.HasContent())
            {
                // If there is actually content, go to the root.
                return Redirect("~/");
            }

            var model = new NoNodesViewModel
            {
                SbnPath = _ioHelper.ResolveUrl(_globalSettings.Value.SbnPath),
            };

            return View(_globalSettings.Value.NoNodesViewPath, model);
        }
    }
}
