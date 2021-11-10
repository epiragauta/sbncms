namespace Sbn.Cms.Core.Models
{
    public interface IPartialView : IFile
    {
        PartialViewType ViewType { get; }
    }
}
