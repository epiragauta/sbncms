using System;

namespace Sbn.Cms.Core.CodeAnnotations
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SbnUdiTypeAttribute : Attribute
    {
        public string UdiType { get; private set; }

        public SbnUdiTypeAttribute(string udiType)
        {
            UdiType = udiType;
        }
    }
}
