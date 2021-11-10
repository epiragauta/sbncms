using System.Runtime.Serialization;

namespace Sbn.Cms.Core.Models.ContentEditing
{
    /// <summary>
    /// A model representing a media item to be saved
    /// </summary>
    [DataContract(Name = "content", Namespace = "")]
    public class MediaItemSave : ContentBaseSave<IMedia>
    {
    }
}
