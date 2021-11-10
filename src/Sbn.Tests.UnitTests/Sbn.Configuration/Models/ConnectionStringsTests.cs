// Copyright (c) Sbn.
// See LICENSE for more details.

using NUnit.Framework;
using Sbn.Cms.Core.Configuration;
using Sbn.Cms.Core.Configuration.Models;
using Constants = Sbn.Cms.Core.Constants;

namespace Sbn.Cms.Tests.UnitTests.Sbn.Configuration.Models
{
    public class ConnectionStringsTests
    {
        [Test]
        [TestCase("", ExpectedResult = null)]
        [TestCase(null, ExpectedResult = null)]
        [TestCase(@"Data Source=|DataDirectory|\Sbn.sdf;Flush Interval=1;", ExpectedResult = Constants.DbProviderNames.SqlCe)]
        [TestCase(@"Server=(LocalDb)\Sbn;Database=NetCore;Integrated Security=true", ExpectedResult = Constants.DbProviderNames.SqlServer)]
        [TestCase(@"Data Source=(LocalDb)\Sbn;Initial Catalog=NetCore;Integrated Security=true;", ExpectedResult = Constants.DbProviderNames.SqlServer)]
        [TestCase(@"Data Source=.\SQLExpress;Integrated Security=true;AttachDbFilename=MyDataFile.mdf;", ExpectedResult = Constants.DbProviderNames.SqlServer)]
        public string ParseProviderName(string connectionString)
        {
            var connectionStrings = new ConnectionStrings
            {
                SbnConnectionString = new ConfigConnectionString(Constants.System.SbnConnectionName, connectionString)
            };

            var actual = connectionStrings.SbnConnectionString;

            Assert.AreEqual(connectionString, actual.ConnectionString);
            Assert.AreEqual(Constants.System.SbnConnectionName, actual.Name);

            return connectionStrings.SbnConnectionString.ProviderName;
        }
    }
}
