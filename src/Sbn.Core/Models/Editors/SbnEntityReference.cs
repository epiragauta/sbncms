using System;
using System.Collections.Generic;

namespace Sbn.Cms.Core.Models.Editors
{
    /// <summary>
    /// Used to track reference to other entities in a property value
    /// </summary>
    public struct SbnEntityReference : IEquatable<SbnEntityReference>
    {
        private static readonly SbnEntityReference _empty = new SbnEntityReference(UnknownTypeUdi.Instance, string.Empty);

        public SbnEntityReference(Udi udi, string relationTypeAlias)
        {
            Udi = udi ?? throw new ArgumentNullException(nameof(udi));
            RelationTypeAlias = relationTypeAlias ?? throw new ArgumentNullException(nameof(relationTypeAlias));
        }

        public SbnEntityReference(Udi udi)
        {
            Udi = udi ?? throw new ArgumentNullException(nameof(udi));

            switch (udi.EntityType)
            {
                case Constants.UdiEntityType.Media:
                    RelationTypeAlias = Constants.Conventions.RelationTypes.RelatedMediaAlias;
                    break;
                default:
                    RelationTypeAlias = Constants.Conventions.RelationTypes.RelatedDocumentAlias;
                    break;
            }
        }

        public static SbnEntityReference Empty() => _empty;

        public static bool IsEmpty(SbnEntityReference reference) => reference == Empty();

        public Udi Udi { get; }
        public string RelationTypeAlias { get; }

        public override bool Equals(object obj)
        {
            return obj is SbnEntityReference reference && Equals(reference);
        }

        public bool Equals(SbnEntityReference other)
        {
            return EqualityComparer<Udi>.Default.Equals(Udi, other.Udi) &&
                   RelationTypeAlias == other.RelationTypeAlias;
        }

        public override int GetHashCode()
        {
            var hashCode = -487348478;
            hashCode = hashCode * -1521134295 + EqualityComparer<Udi>.Default.GetHashCode(Udi);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(RelationTypeAlias);
            return hashCode;
        }

        public static bool operator ==(SbnEntityReference left, SbnEntityReference right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SbnEntityReference left, SbnEntityReference right)
        {
            return !(left == right);
        }
    }
}
