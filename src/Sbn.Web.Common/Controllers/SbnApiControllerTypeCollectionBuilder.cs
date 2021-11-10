using Sbn.Cms.Core;
using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Web.Common.Controllers
{
    public class SbnApiControllerTypeCollectionBuilder : TypeCollectionBuilderBase<SbnApiControllerTypeCollectionBuilder, SbnApiControllerTypeCollection, SbnApiController>
    {
        // TODO: Should this only exist in the back office project? These really are only ever used for the back office AFAIK

        protected override SbnApiControllerTypeCollectionBuilder This => this;
    }
}
