namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.Common
{
    public class DeleteKeysAndIndexes : MigrationBase
    {
        public DeleteKeysAndIndexes(IMigrationContext context)
            : base(context)
        { }

        protected override void Migrate()
        {
            // all v7.14 tables
            var tables = new[]
            {
                "cmsContent",
                "cmsContentType",
                "cmsContentType2ContentType",
                "cmsContentTypeAllowedContentType",
                "cmsContentVersion",
                "cmsContentXml",
                "cmsDataType",
                "cmsDataTypePreValues",
                "cmsDictionary",
                "cmsDocument",
                "cmsDocumentType",
                "cmsLanguageText",
                "cmsMacro",
                "cmsMacroProperty",
                "cmsMedia",
                "cmsMember",
                "cmsMember2MemberGroup",
                "cmsMemberType",
                "cmsPreviewXml",
                "cmsPropertyData",
                "cmsPropertyType",
                "cmsPropertyTypeGroup",
                "cmsTagRelationship",
                "cmsTags",
                "cmsTask",
                "cmsTaskType",
                "cmsTemplate",
                "sbnAccess",
                "sbnAccessRule",
                "sbnAudit",
                "sbnCacheInstruction",
                "sbnConsent",
                "sbnDomains",
                "sbnExternalLogin",
                "sbnLanguage",
                "sbnLock",
                "sbnLog",
                "sbnMigration",
                "sbnNode",
                "sbnRedirectUrl",
                "sbnRelation",
                "sbnRelationType",
                "sbnServer",
                "sbnUser",
                "sbnUser2NodeNotify",
                "sbnUser2UserGroup",
                "sbnUserGroup",
                "sbnUserGroup2App",
                "sbnUserGroup2NodePermission",
                "sbnUserLogin",
                "sbnUserStartNode",
            };

            // delete *all* keys and indexes - because of FKs
            // on known v7 tables only
            foreach (var table in tables)
                Delete.KeysAndIndexes(table, false, true).Do();
            foreach (var table in tables)
                Delete.KeysAndIndexes(table, true, false).Do();
        }
    }
}
