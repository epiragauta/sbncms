using Sbn.Cms.Core.Composing;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core.PropertyEditors
{
    public class MediaUrlGeneratorCollectionBuilder : SetCollectionBuilderBase<MediaUrlGeneratorCollectionBuilder, MediaUrlGeneratorCollection, IMediaUrlGenerator>
    {
        protected override MediaUrlGeneratorCollectionBuilder This => this;
    }
}
