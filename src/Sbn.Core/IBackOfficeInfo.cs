namespace Sbn.Cms.Core
{
    public interface IBackOfficeInfo
    {
        /// <summary>
        /// Gets the absolute url to the Sbn Backoffice. This info can be used to build absolute urls for Backoffice to use in mails etc.
        /// </summary>
        string GetAbsoluteUrl { get; }
    }
}
