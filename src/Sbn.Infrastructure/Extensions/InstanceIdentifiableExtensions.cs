using System;
using System.Collections.Generic;
using System.Text;
using Sbn.Cms.Core.Scoping;

namespace Sbn.Extensions
{
    internal static class InstanceIdentifiableExtensions
    {
        public static string GetDebugInfo(this IInstanceIdentifiable instance)
        {
            if (instance == null)
            {
                return "(NULL)";
            }

            return $"(id: {instance.InstanceId.ToString("N").Substring(0, 8)} from thread: {instance.CreatedThreadId})";
        }
    }
}
