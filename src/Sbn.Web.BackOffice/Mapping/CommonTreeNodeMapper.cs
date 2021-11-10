using Microsoft.AspNetCore.Routing;
using Sbn.Cms.Core.Models;
using Sbn.Cms.Web.BackOffice.Trees;
using Sbn.Cms.Web.Common.Controllers;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.Mapping
{
    public class CommonTreeNodeMapper
    {
        private readonly LinkGenerator _linkGenerator;


        public CommonTreeNodeMapper( LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }


        public string GetTreeNodeUrl<TController>(IContentBase source)
            where TController : SbnApiController, ITreeNodeController
        {
            return _linkGenerator.GetSbnApiService<TController>(controller => controller.GetTreeNode(source.Key.ToString("N"), null));
        }

    }
}
