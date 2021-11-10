// Copyright (c) Sbn.
// See LICENSE for more details.

using Microsoft.AspNetCore.Authorization;

namespace Sbn.Cms.Web.BackOffice.Authorization
{
    /// <summary>
    /// Marker requirement for the <see cref="DenyLocalLoginHandler"/>.
    /// </summary>
    public class DenyLocalLoginRequirement : IAuthorizationRequirement
    {
    }
}
