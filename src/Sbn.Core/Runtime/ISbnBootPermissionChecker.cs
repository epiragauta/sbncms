namespace Sbn.Cms.Core.Runtime
{
    public interface ISbnBootPermissionChecker
    {
        void ThrowIfNotPermissions();
    }
}
