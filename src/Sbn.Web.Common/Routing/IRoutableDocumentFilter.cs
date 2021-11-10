namespace Sbn.Cms.Web.Common.Routing
{
    public interface IRoutableDocumentFilter
    {
        bool IsDocumentRequest(string absPath);
    }
}
