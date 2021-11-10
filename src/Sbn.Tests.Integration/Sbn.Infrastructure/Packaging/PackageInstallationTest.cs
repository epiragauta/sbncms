// Copyright (c) Sbn.
// See LICENSE for more details.

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Sbn.Cms.Core.Hosting;
using Sbn.Cms.Core.Models.Packaging;
using Sbn.Cms.Core.Packaging;
using Sbn.Cms.Infrastructure.Packaging;
using Sbn.Cms.Tests.Common.Testing;
using Sbn.Cms.Tests.Integration.Testing;

namespace Sbn.Cms.Tests.Integration.Sbn.Infrastructure.Packaging
{
    [TestFixture]
    [SbnTest(Database = SbnTestOptions.Database.NewSchemaPerFixture)]
    public class PackageInstallationTest : SbnIntegrationTest
    {
        private IHostingEnvironment HostingEnvironment => GetRequiredService<IHostingEnvironment>();

        private PackageInstallation PackageInstallation => (PackageInstallation)GetRequiredService<IPackageInstallation>();

        private const string DocumentTypePickerPackage = "Document_Type_Picker_1.1.package.xml";
        private const string HelloPackage = "Hello_1.0.0.package.xml";

        [Test]
        public void Can_Read_Compiled_Package_1()
        {
            var testPackageFile = new FileInfo(Path.Combine(HostingEnvironment.MapPathContentRoot("~/TestData/Packages"), DocumentTypePickerPackage));
            using var fileStream = testPackageFile.OpenRead();
            CompiledPackage package = PackageInstallation.ReadPackage(XDocument.Load(fileStream));
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(package);
                Assert.AreEqual("Document Type Picker", package.Name);
                Assert.AreEqual(1, package.DataTypes.Count());
            });
        }

        [Test]
        public void Can_Read_Compiled_Package_2()
        {
            var testPackageFile = new FileInfo(Path.Combine(HostingEnvironment.MapPathContentRoot("~/TestData/Packages"), HelloPackage));
            using var fileStream = testPackageFile.OpenRead();
            CompiledPackage package = PackageInstallation.ReadPackage(XDocument.Load(fileStream));
            Assert.Multiple(() =>
            {
                Assert.IsNotNull(package);
                Assert.AreEqual("Hello", package.Name);
                Assert.AreEqual(1, package.Documents.Count());
                Assert.AreEqual(1, package.DocumentTypes.Count());
                Assert.AreEqual(1, package.Templates.Count());
                Assert.AreEqual(1, package.DataTypes.Count());
            });
        }

        [Test]
        public void Can_Read_Compiled_Package_Warnings()
        {
            // Copy a file to the same path that the package will install so we can detect file conflicts.
            string filePath = Path.Combine(HostingEnvironment.MapPathContentRoot("~/"), "bin", "Auros.DocumentTypePicker.dll");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, "test");

            // this is where our test zip file is
            string packageFile = Path.Combine(HostingEnvironment.MapPathContentRoot("~/TestData/Packages"), DocumentTypePickerPackage);
            Console.WriteLine(packageFile);

            using var fileStream = File.OpenRead(packageFile);
            CompiledPackage package = PackageInstallation.ReadPackage(XDocument.Load(fileStream));
            InstallWarnings preInstallWarnings = package.Warnings;
            var dataType = package.DataTypes.First();
            Assert.Multiple(() =>
            {
                Assert.AreEqual("Document Type Picker", package.Name);
                Assert.AreEqual("3593d8e7-8b35-47b9-beda-5e830ca8c93c", dataType.LastAttribute?.Value);
                Assert.AreEqual("Document Type Picker", dataType.FirstAttribute?.Value);
                Assert.IsNotNull(preInstallWarnings);
                Assert.AreEqual(0, preInstallWarnings.ConflictingMacros.Count());
                Assert.AreEqual(0, preInstallWarnings.ConflictingStylesheets.Count());
                Assert.AreEqual(0, preInstallWarnings.ConflictingTemplates.Count());
            });
        }

        [Test]
        public void Install_Data()
        {
            var testPackageFile = new FileInfo(Path.Combine(HostingEnvironment.MapPathContentRoot("~/TestData/Packages"), DocumentTypePickerPackage));
            using var fileStream = testPackageFile.OpenRead();
            CompiledPackage package = PackageInstallation.ReadPackage(XDocument.Load(fileStream));

            InstallationSummary summary = PackageInstallation.InstallPackageData(package, -1, out PackageDefinition def);

            Assert.AreEqual(1, summary.DataTypesInstalled.Count());

            // make sure the def is updated too
            Assert.AreEqual(summary.DataTypesInstalled.Count(), def.DataTypes.Count);
        }
    }
}
