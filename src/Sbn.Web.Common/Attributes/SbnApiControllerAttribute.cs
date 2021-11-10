using System;
using Sbn.Cms.Web.Common.ApplicationModels;

namespace Sbn.Cms.Web.Common.Attributes
{
    /// <summary>
    /// When present on a controller then <see cref="SbnApiBehaviorApplicationModelProvider"/> conventions will apply
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class SbnApiControllerAttribute : Attribute
    {
    }
}
