using Microsoft.AspNetCore.Routing;
using Sbn.Cms.Web.Common.ModelsBuilder;
using Sbn.Extensions;

namespace Sbn.Cms.Web.BackOffice.ModelsBuilder
{
    public class ModelsBuilderDashboardProvider: IModelsBuilderDashboardProvider
    {
        private readonly LinkGenerator _linkGenerator;

        public ModelsBuilderDashboardProvider(LinkGenerator linkGenerator)
        {
            _linkGenerator = linkGenerator;
        }

        public string GetUrl() =>
            _linkGenerator.GetSbnApiServiceBaseUrl<ModelsBuilderDashboardController>(controller =>
                controller.BuildModels());
    }
}
