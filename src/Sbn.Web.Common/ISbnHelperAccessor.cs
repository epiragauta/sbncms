namespace Sbn.Cms.Web.Common
{
    public interface ISbnHelperAccessor
    {
        bool TryGetSbnHelper(out SbnHelper sbnHelper);
    }
}
