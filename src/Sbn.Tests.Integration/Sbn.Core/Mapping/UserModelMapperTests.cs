// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Sbn.Cms.Core.Mapping;
using Sbn.Cms.Core.Models.ContentEditing;
using Sbn.Cms.Core.Models.Membership;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Core.Mapping
{
    [TestFixture]
    [SbnTest(Mapper = true, Database = SbnTestOptions.Database.NewSchemaPerTest)]
    public class UserModelMapperTests : SbnIntegrationTest
    {
        private ISbnMapper _sut;

        [SetUp]
        public void Setup() => _sut = Services.GetRequiredService<ISbnMapper>();

        [Test]
        public void Map_UserGroupSave_To_IUserGroup()
        {
            IUserGroup userGroup = new UserGroup(ShortStringHelper, 0, "alias", "name", new List<string> { "c" }, "icon")
            {
                Id = 42
            };

            // userGroup.permissions is List`1[System.String]

            // userGroup.permissions is System.Linq.Enumerable+WhereSelectArrayIterator`2[System.Char, System.String]
            // fixed: now List`1[System.String]
            const string json = "{\"id\":@@@ID@@@,\"alias\":\"perm1\",\"name\":\"Perm1\",\"icon\":\"icon-users\",\"sections\":[\"content\"],\"users\":[],\"defaultPermissions\":[\"F\",\"C\",\"A\"],\"assignedPermissions\":{},\"startContentId\":-1,\"startMediaId\":-1,\"action\":\"save\",\"parentId\":-1}";
            UserGroupSave userGroupSave = JsonConvert.DeserializeObject<UserGroupSave>(json.Replace("@@@ID@@@", userGroup.Id.ToString()));

            // failed, AutoMapper complained, "Unable to cast object of type 'WhereSelectArrayIterator`2[System.Char,System.String]' to type 'System.Collections.IList'".
            // FIXME: added ToList() in UserGroupFactory
            _sut.Map(userGroupSave, userGroup);
        }
    }
}
