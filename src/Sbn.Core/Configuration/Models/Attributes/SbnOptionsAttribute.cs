using System;

namespace Sbn.Cms.Core.Configuration.Models
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SbnOptionsAttribute : Attribute
    {
        public string ConfigurationKey { get; }
        public bool BindNonPublicProperties { get; set; }

        public SbnOptionsAttribute(string configurationKey)
        {
            ConfigurationKey = configurationKey;
        }
    }
}
