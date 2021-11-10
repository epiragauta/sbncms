using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Events;
using Sbn.Cms.Infrastructure.Migrations.Install;
using Sbn.Cms.Infrastructure.Persistence.Dtos;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Persistence
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class SqlServerTableByTableTest : SbnIntegrationTest
    {
        private ISbnVersion SbnVersion => GetRequiredService<ISbnVersion>();
        private static ILoggerFactory _loggerFactory = NullLoggerFactory.Instance;
        private IEventAggregator EventAggregator => GetRequiredService<IEventAggregator>();

        [Test]
        public void Can_Create_sbnNode_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnAccess_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<AccessDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnAccessRule_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<AccessDto>();
                helper.CreateTable<AccessRuleDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsContentType2ContentType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentType2ContentTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsContentTypeAllowedContentType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<ContentTypeAllowedContentTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsContentType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_ContentVersion_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<ContentDto>();
                helper.CreateTable<ContentVersionDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsDataType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<DataTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsDictionary_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<DictionaryDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsLanguageText_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<DictionaryDto>();
                helper.CreateTable<LanguageDto>();
                helper.CreateTable<LanguageTextDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsTemplate_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<TemplateDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_Document_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<ContentDto>();
                helper.CreateTable<TemplateDto>();
                helper.CreateTable<DocumentDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_DocumentType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<TemplateDto>();
                helper.CreateTable<ContentTypeTemplateDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnDomains_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<DomainDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnLogViewerQuery_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<LogViewerQueryDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnLanguage_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<LanguageDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnLog_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<LogDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsMacro_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<MacroDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsMember_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<ContentDto>();
                helper.CreateTable<MemberDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsMember2MemberGroup_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<ContentDto>();
                helper.CreateTable<MemberDto>();
                helper.CreateTable<Member2MemberGroupDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsMemberType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<MemberPropertyTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_PropertyData_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<DataTypeDto>();
                helper.CreateTable<PropertyTypeGroupDto>();
                helper.CreateTable<PropertyTypeDto>();
                helper.CreateTable<PropertyDataDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsPropertyType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<DataTypeDto>();
                helper.CreateTable<PropertyTypeGroupDto>();
                helper.CreateTable<PropertyTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsPropertyTypeGroup_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<PropertyTypeGroupDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnRelation_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<RelationTypeDto>();
                helper.CreateTable<RelationDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnRelationType_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<RelationTypeDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsTags_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<TagDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_cmsTagRelationship_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<ContentDto>();
                helper.CreateTable<ContentTypeDto>();
                helper.CreateTable<DataTypeDto>();
                helper.CreateTable<PropertyTypeGroupDto>();
                helper.CreateTable<PropertyTypeDto>();
                helper.CreateTable<TagDto>();
                helper.CreateTable<TagRelationshipDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnUser_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<UserDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnUserGroup_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<UserGroupDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnUser2NodeNotify_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<UserDto>();
                helper.CreateTable<User2NodeNotifyDto>();

                scope.Complete();
            }
        }

        public void Can_Create_sbnGroupUser2app_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<UserGroupDto>();
                helper.CreateTable<UserGroup2AppDto>();

                scope.Complete();
            }
        }

        [Test]
        public void Can_Create_sbnUserGroup2NodePermission_Table()
        {
            using (var scope = ScopeProvider.CreateScope())
            {
                var helper = new DatabaseSchemaCreator(scope.Database, _loggerFactory.CreateLogger<DatabaseSchemaCreator>(), _loggerFactory, SbnVersion, EventAggregator);

                helper.CreateTable<NodeDto>();
                helper.CreateTable<UserGroupDto>();
                helper.CreateTable<UserGroup2NodePermissionDto>();

                scope.Complete();
            }
        }
    }
}
