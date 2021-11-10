using System;
using Sbn.Cms.Core.Models;

namespace Sbn.Cms.Core
{
    public static class UdiEntityTypeHelper
    {


        public static string FromSbnObjectType(SbnObjectTypes sbnObjectType)
        {
            switch (sbnObjectType)
            {
                case SbnObjectTypes.Document:
                    return Constants.UdiEntityType.Document;
                case SbnObjectTypes.DocumentBlueprint:
                    return Constants.UdiEntityType.DocumentBlueprint;
                case SbnObjectTypes.Media:
                    return Constants.UdiEntityType.Media;
                case SbnObjectTypes.Member:
                    return Constants.UdiEntityType.Member;
                case SbnObjectTypes.Template:
                    return Constants.UdiEntityType.Template;
                case SbnObjectTypes.DocumentType:
                    return Constants.UdiEntityType.DocumentType;
                case SbnObjectTypes.DocumentTypeContainer:
                    return Constants.UdiEntityType.DocumentTypeContainer;
                case SbnObjectTypes.MediaType:
                    return Constants.UdiEntityType.MediaType;
                case SbnObjectTypes.MediaTypeContainer:
                    return Constants.UdiEntityType.MediaTypeContainer;
                case SbnObjectTypes.DataType:
                    return Constants.UdiEntityType.DataType;
                case SbnObjectTypes.DataTypeContainer:
                    return Constants.UdiEntityType.DataTypeContainer;
                case SbnObjectTypes.MemberType:
                    return Constants.UdiEntityType.MemberType;
                case SbnObjectTypes.MemberGroup:
                    return Constants.UdiEntityType.MemberGroup;
                case SbnObjectTypes.RelationType:
                    return Constants.UdiEntityType.RelationType;
                case SbnObjectTypes.FormsForm:
                    return Constants.UdiEntityType.FormsForm;
                case SbnObjectTypes.FormsPreValue:
                    return Constants.UdiEntityType.FormsPreValue;
                case SbnObjectTypes.FormsDataSource:
                    return Constants.UdiEntityType.FormsDataSource;
                case SbnObjectTypes.Language:
                    return Constants.UdiEntityType.Language;
            }

            throw new NotSupportedException(
                $"SbnObjectType \"{sbnObjectType}\" does not have a matching EntityType.");
        }

        public static SbnObjectTypes ToSbnObjectType(string entityType)
        {
            switch (entityType)
            {
                case Constants.UdiEntityType.Document:
                    return SbnObjectTypes.Document;
                case Constants.UdiEntityType.DocumentBlueprint:
                    return SbnObjectTypes.DocumentBlueprint;
                case Constants.UdiEntityType.Media:
                    return SbnObjectTypes.Media;
                case Constants.UdiEntityType.Member:
                    return SbnObjectTypes.Member;
                case Constants.UdiEntityType.Template:
                    return SbnObjectTypes.Template;
                case Constants.UdiEntityType.DocumentType:
                    return SbnObjectTypes.DocumentType;
                case Constants.UdiEntityType.DocumentTypeContainer:
                    return SbnObjectTypes.DocumentTypeContainer;
                case Constants.UdiEntityType.MediaType:
                    return SbnObjectTypes.MediaType;
                case Constants.UdiEntityType.MediaTypeContainer:
                    return SbnObjectTypes.MediaTypeContainer;
                case Constants.UdiEntityType.DataType:
                    return SbnObjectTypes.DataType;
                case Constants.UdiEntityType.DataTypeContainer:
                    return SbnObjectTypes.DataTypeContainer;
                case Constants.UdiEntityType.MemberType:
                    return SbnObjectTypes.MemberType;
                case Constants.UdiEntityType.MemberGroup:
                    return SbnObjectTypes.MemberGroup;
                case Constants.UdiEntityType.RelationType:
                    return SbnObjectTypes.RelationType;
                case Constants.UdiEntityType.FormsForm:
                    return SbnObjectTypes.FormsForm;
                case Constants.UdiEntityType.FormsPreValue:
                    return SbnObjectTypes.FormsPreValue;
                case Constants.UdiEntityType.FormsDataSource:
                    return SbnObjectTypes.FormsDataSource;
                case Constants.UdiEntityType.Language:
                    return SbnObjectTypes.Language;
            }

            throw new NotSupportedException(
                $"EntityType \"{entityType}\" does not have a matching SbnObjectType.");
        }
    }
}
