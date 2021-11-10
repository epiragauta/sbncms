namespace Sbn.Cms.Core.Web
{
    public interface ISessionManager
    {
        string GetSessionValue(string sessionName);
        void SetSessionValue(string sessionName, string value);
    }
}
