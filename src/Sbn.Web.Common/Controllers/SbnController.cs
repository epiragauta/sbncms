using System;
using Microsoft.AspNetCore.Mvc;

namespace Sbn.Cms.Web.Common.Controllers
{
    /// <summary>
    /// Provides a base class for Sbn controllers.
    /// </summary>
    public abstract class SbnController : Controller
    {
        // for debugging purposes
        internal Guid InstanceId { get; } = Guid.NewGuid();

    }
}
