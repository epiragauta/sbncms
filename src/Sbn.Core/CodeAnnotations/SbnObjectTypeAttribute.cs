using System;

namespace Sbn.Cms.Core.CodeAnnotations
{
    /// <summary>
    /// Attribute to associate a GUID string and Type with an SbnObjectType Enum value
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class SbnObjectTypeAttribute : Attribute
    {
        public SbnObjectTypeAttribute(string objectId)
        {
            ObjectId = new Guid(objectId);
        }

        public SbnObjectTypeAttribute(string objectId, Type modelType)
        {
            ObjectId = new Guid(objectId);
            ModelType = modelType;
        }

        public Guid ObjectId { get; private set; }

        public Type ModelType { get; private set; }
    }
}
