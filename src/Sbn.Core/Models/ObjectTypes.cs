using System;
using System.Collections.Concurrent;
using System.Reflection;
using Sbn.Cms.Core.CodeAnnotations;

namespace Sbn.Cms.Core.Models
{
    /// <summary>
    /// Provides utilities and extension methods to handle object types.
    /// </summary>
    public static class ObjectTypes
    {
        // must be concurrent to avoid thread collisions!
        private static readonly ConcurrentDictionary<SbnObjectTypes, Guid> SbnGuids = new ConcurrentDictionary<SbnObjectTypes, Guid>();
        private static readonly ConcurrentDictionary<SbnObjectTypes, string> SbnUdiTypes = new ConcurrentDictionary<SbnObjectTypes, string>();
        private static readonly ConcurrentDictionary<SbnObjectTypes, string> SbnFriendlyNames = new ConcurrentDictionary<SbnObjectTypes, string>();
        private static readonly ConcurrentDictionary<SbnObjectTypes, Type> SbnTypes = new ConcurrentDictionary<SbnObjectTypes, Type>();
        private static readonly ConcurrentDictionary<Guid, string> GuidUdiTypes = new ConcurrentDictionary<Guid, string>();
        private static readonly ConcurrentDictionary<Guid, SbnObjectTypes> GuidObjectTypes = new ConcurrentDictionary<Guid, SbnObjectTypes>();
        private static readonly ConcurrentDictionary<Guid, Type> GuidTypes = new ConcurrentDictionary<Guid, Type>();

        private static FieldInfo GetEnumField(string name)
        {
            return typeof (SbnObjectTypes).GetField(name, BindingFlags.Public | BindingFlags.Static);
        }

        private static FieldInfo GetEnumField(Guid guid)
        {
            var fields = typeof (SbnObjectTypes).GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttribute<SbnObjectTypeAttribute>(false);
                if (attribute != null && attribute.ObjectId == guid) return field;
            }

            return null;
        }

        /// <summary>
        /// Gets the Sbn object type corresponding to a name.
        /// </summary>
        public static SbnObjectTypes GetSbnObjectType(string name)
        {
            return (SbnObjectTypes) Enum.Parse(typeof (SbnObjectTypes), name, true);
        }

        #region Guid object type utilities

        /// <summary>
        /// Gets the Sbn object type corresponding to an object type Guid.
        /// </summary>
        public static SbnObjectTypes GetSbnObjectType(Guid objectType)
        {
            return GuidObjectTypes.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(objectType);
                if (field == null) return SbnObjectTypes.Unknown;

                return (SbnObjectTypes) field.GetValue(null);
            });
        }

        /// <summary>
        /// Gets the Udi type corresponding to an object type Guid.
        /// </summary>
        public static string GetUdiType(Guid objectType)
        {
            return GuidUdiTypes.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(objectType);
                if (field == null) return Constants.UdiEntityType.Unknown;

                var attribute = field.GetCustomAttribute<SbnUdiTypeAttribute>(false);
                return attribute?.UdiType ?? Constants.UdiEntityType.Unknown;
            });
        }

        /// <summary>
        /// Gets the CLR type corresponding to an object type Guid.
        /// </summary>
        public static Type GetClrType(Guid objectType)
        {
            return GuidTypes.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(objectType);
                if (field == null) return null;

                var attribute = field.GetCustomAttribute<SbnObjectTypeAttribute>(false);
                return attribute?.ModelType;
            });
        }

        #endregion

        #region SbnObjectTypes extension methods

        /// <summary>
        /// Gets the object type Guid corresponding to this Sbn object type.
        /// </summary>
        public static Guid GetGuid(this SbnObjectTypes objectType)
        {
            return SbnGuids.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(t.ToString());
                var attribute = field.GetCustomAttribute<SbnObjectTypeAttribute>(false);

                return attribute?.ObjectId ?? Guid.Empty;
            });
        }

        /// <summary>
        /// Gets the Udi type corresponding to this Sbn object type.
        /// </summary>
        public static string GetUdiType(this SbnObjectTypes objectType)
        {
            return SbnUdiTypes.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(t.ToString());
                var attribute = field.GetCustomAttribute<SbnUdiTypeAttribute>(false);

                return attribute?.UdiType ?? Constants.UdiEntityType.Unknown;
            });
        }

        /// <summary>
        /// Gets the name corresponding to this Sbn object type.
        /// </summary>
        public static string GetName(this SbnObjectTypes objectType)
        {
            return Enum.GetName(typeof (SbnObjectTypes), objectType);
        }

        /// <summary>
        /// Gets the friendly name corresponding to this Sbn object type.
        /// </summary>
        public static string GetFriendlyName(this SbnObjectTypes objectType)
        {
            return SbnFriendlyNames.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(t.ToString());
                var attribute = field.GetCustomAttribute<FriendlyNameAttribute>(false);

                return attribute?.ToString() ?? string.Empty;
            });
        }

        /// <summary>
        /// Gets the CLR type corresponding to this Sbn object type.
        /// </summary>
        public static Type GetClrType(this SbnObjectTypes objectType)
        {
            return SbnTypes.GetOrAdd(objectType, t =>
            {
                var field = GetEnumField(t.ToString());
                var attribute = field.GetCustomAttribute<SbnObjectTypeAttribute>(false);

                return attribute?.ModelType;
            });
        }

        #endregion
    }
}
