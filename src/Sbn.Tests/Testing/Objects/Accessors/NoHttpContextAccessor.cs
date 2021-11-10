using System.Web;
using Sbn.Web;

namespace Sbn.Tests.Testing.Objects.Accessors
{
    public class NoHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContextBase HttpContext { get; set; } = null;
    }
}
