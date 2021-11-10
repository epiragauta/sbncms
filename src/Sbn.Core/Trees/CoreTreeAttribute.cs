using System;

namespace Sbn.Cms.Core.Trees
{
    /// <summary>
    /// Indicates that a tree is a core tree and should not be treated as a plugin tree.
    /// </summary>
    /// <remarks>
    /// This ensures that sbn will look in the sbn folders for views for this tree.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class CoreTreeAttribute : Attribute
    { }
}
