namespace Sbn.Cms.Infrastructure.Migrations.Upgrade.V_8_0_0
{
    public class SuperZero : MigrationBase
    {
        public SuperZero(IMigrationContext context)
            : base(context)
        { }

        protected override void Migrate()
        {
            var exists = Database.Fetch<int>("select id from sbnUser where id=-1;").Count > 0;
            if (exists) return;

            Database.Execute("update sbnUser set userLogin = userLogin + '__' where id=0");

            Database.Execute("set identity_insert sbnUser on;");
            Database.Execute(@"
                insert into sbnUser (id,
                    userDisabled, userNoConsole, userName, userLogin, userPassword, passwordConfig,
                    userEmail, userLanguage, securityStampToken, failedLoginAttempts, lastLockoutDate,
	                lastPasswordChangeDate, lastLoginDate, emailConfirmedDate, invitedDate,
	                createDate, updateDate, avatar, tourData)
                select
                    -1 id,
                    userDisabled, userNoConsole, userName, substring(userLogin, 1, len(userLogin) - 2) userLogin, userPassword, passwordConfig,
	                userEmail, userLanguage, securityStampToken, failedLoginAttempts, lastLockoutDate,
	                lastPasswordChangeDate, lastLoginDate, emailConfirmedDate, invitedDate,
	                createDate, updateDate, avatar, tourData
                from sbnUser where id=0;");
            Database.Execute("set identity_insert sbnUser off;");

            Database.Execute("update sbnUser2UserGroup set userId=-1 where userId=0;");
            Database.Execute("update sbnUser2NodeNotify set userId=-1 where userId=0;");
            Database.Execute("update sbnNode set nodeUser=-1 where nodeUser=0;");
            Database.Execute("update sbnUserLogin set userId=-1 where userId=0;");
            Database.Execute($"update {Cms.Core.Constants.DatabaseSchema.Tables.ContentVersion} set userId=-1 where userId=0;");
            Database.Execute("delete from sbnUser where id=0;");
        }
    }
}
