using System.IO;
using System.Threading.Tasks;

namespace Sbn.Cms.Core.Templates
{
    /// <summary>
    /// This is used purely for the RenderTemplate functionality in Sbn
    /// </summary>
    public interface ITemplateRenderer
    {
        Task RenderAsync(int pageId, int? altTemplateId, StringWriter writer);
    }
}
