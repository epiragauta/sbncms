namespace Sbn.Cms.Web.Common.Models
{
    /// <summary>
    /// A base model containing a value to indicate to Sbn where to redirect to after Posting if
    /// a developer doesn't want the controller to redirect to the current Sbn page - which is the default.
    /// </summary>
    public class PostRedirectModel
    {
        /// <summary>
        /// The path to redirect to when update is successful, if not specified then the user will be
        /// redirected to the current Sbn page
        /// </summary>
        public string RedirectUrl { get; set; }
    }
}
