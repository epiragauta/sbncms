using Sbn.Cms.Core.Composing;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// A marker interface to designate that a controller will be used for Sbn front-end requests and/or route hijacking
    /// </summary>
    public interface IRenderController : IDiscoverable
    {

    }
}
