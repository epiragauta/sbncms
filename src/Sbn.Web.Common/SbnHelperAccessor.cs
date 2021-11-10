using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Sbn.Cms.Web.Common
{
    public class SbnHelperAccessor : ISbnHelperAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SbnHelperAccessor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

        public bool TryGetSbnHelper(out SbnHelper sbnHelper)
        {
            sbnHelper = _httpContextAccessor.HttpContext?.RequestServices.GetService<SbnHelper>();
            return sbnHelper is not null;
        }
    }
}
