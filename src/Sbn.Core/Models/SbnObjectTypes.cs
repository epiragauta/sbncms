using Sbn.Cms.Core.CodeAnnotations;

namespace Sbn.Cms.Core.Models
{
    /// <summary>
    /// Enum used to represent the Sbn Object Types and their associated GUIDs
    /// </summary>
    public enum SbnObjectTypes
    {
        /// <summary>
        /// Default value
        /// </summary>
        Unknown,


        /// <summary>
        /// Root
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.SystemRoot)]
        [FriendlyName("Root")]
        ROOT,

        /// <summary>
        /// Document
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.Document, typeof(IContent))]
        [FriendlyName("Document")]
        [SbnUdiType(Constants.UdiEntityType.Document)]
        Document,

        /// <summary>
        /// Media
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.Media, typeof(IMedia))]
        [FriendlyName("Media")]
        [SbnUdiType(Constants.UdiEntityType.Media)]
        Media,

        /// <summary>
        /// Member Type
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.MemberType, typeof(IMemberType))]
        [FriendlyName("Member Type")]
        [SbnUdiType(Constants.UdiEntityType.MemberType)]
        MemberType,

        /// <summary>
        /// Template
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.Template, typeof(ITemplate))]
        [FriendlyName("Template")]
        [SbnUdiType(Constants.UdiEntityType.Template)]
        Template,

        /// <summary>
        /// Member Group
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.MemberGroup)]
        [FriendlyName("Member Group")]
        [SbnUdiType(Constants.UdiEntityType.MemberGroup)]
        MemberGroup,

        /// <summary>
        /// "Media Type
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.MediaType, typeof(IMediaType))]
        [FriendlyName("Media Type")]
        [SbnUdiType(Constants.UdiEntityType.MediaType)]
        MediaType,

        /// <summary>
        /// Document Type
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.DocumentType, typeof(IContentType))]
        [FriendlyName("Document Type")]
        [SbnUdiType(Constants.UdiEntityType.DocumentType)]
        DocumentType,

        /// <summary>
        /// Recycle Bin
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.ContentRecycleBin)]
        [FriendlyName("Recycle Bin")]
        RecycleBin,

        /// <summary>
        /// Member
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.Member, typeof(IMember))]
        [FriendlyName("Member")]
        [SbnUdiType(Constants.UdiEntityType.Member)]
        Member,

        /// <summary>
        /// Data Type
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.DataType, typeof(IDataType))]
        [FriendlyName("Data Type")]
        [SbnUdiType(Constants.UdiEntityType.DataType)]
        DataType,

        /// <summary>
        /// Document type container
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.DocumentTypeContainer)]
        [FriendlyName("Document Type Container")]
        [SbnUdiType(Constants.UdiEntityType.DocumentTypeContainer)]
        DocumentTypeContainer,

        /// <summary>
        /// Media type container
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.MediaTypeContainer)]
        [FriendlyName("Media Type Container")]
        [SbnUdiType(Constants.UdiEntityType.MediaTypeContainer)]
        MediaTypeContainer,

        /// <summary>
        /// Media type container
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.DataTypeContainer)]
        [FriendlyName("Data Type Container")]
        [SbnUdiType(Constants.UdiEntityType.DataTypeContainer)]
        DataTypeContainer,

        /// <summary>
        /// Relation type
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.RelationType)]
        [FriendlyName("Relation Type")]
        [SbnUdiType(Constants.UdiEntityType.RelationType)]
        RelationType,

        /// <summary>
        /// Forms Form
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.FormsForm)]
        [FriendlyName("Form")]
        FormsForm,

        /// <summary>
        /// Forms PreValue
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.FormsPreValue)]
        [FriendlyName("PreValue")]
        FormsPreValue,

        /// <summary>
        /// Forms DataSource
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.FormsDataSource)]
        [FriendlyName("DataSource")]
        FormsDataSource,

        /// <summary>
        /// Language
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.Language)]
        [FriendlyName("Language")]
        Language,

        /// <summary>
        /// Document Blueprint
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.DocumentBlueprint, typeof(IContent))]
        [FriendlyName("DocumentBlueprint")]
        [SbnUdiType(Constants.UdiEntityType.DocumentBlueprint)]
        DocumentBlueprint,

        /// <summary>
        /// Reserved Identifier
        /// </summary>
        [SbnObjectType(Constants.ObjectTypes.Strings.IdReservation)]
        [FriendlyName("Identifier Reservation")]
        IdReservation

    }
}
