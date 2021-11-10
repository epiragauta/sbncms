using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Sbn.Cms.Core.Models.Packaging;
using Sbn.Cms.Core.Packaging;

namespace Sbn.Cms.Core.Services
{
    public interface IPackagingService : IService
    {
        /// <summary>
        /// Returns a <see cref="CompiledPackage"/> result from an sbn package file (zip)
        /// </summary>
        /// <param name="packageFile"></param>
        /// <returns></returns>
        CompiledPackage GetCompiledPackageInfo(XDocument packageXml);

        /// <summary>
        /// Installs the data, entities, objects contained in an sbn package file (zip)
        /// </summary>
        /// <param name="packageFile"></param>
        /// <param name="userId"></param>
        InstallationSummary InstallCompiledPackageData(FileInfo packageXmlFile, int userId = Constants.Security.SuperUserId);

        InstallationSummary InstallCompiledPackageData(XDocument packageXml, int userId = Constants.Security.SuperUserId);

        /// <summary>
        /// Returns the advertised installed packages
        /// </summary>
        /// <returns></returns>
        IEnumerable<InstalledPackage> GetAllInstalledPackages();

        InstalledPackage GetInstalledPackageByName(string packageName);

        /// <summary>
        /// Returns the created packages
        /// </summary>
        /// <returns></returns>
        IEnumerable<PackageDefinition> GetAllCreatedPackages();

        /// <summary>
        /// Returns a created package by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PackageDefinition GetCreatedPackageById(int id);

        void DeleteCreatedPackage(int id, int userId = Constants.Security.SuperUserId);

        /// <summary>
        /// Persists a package definition to storage
        /// </summary>
        /// <returns></returns>
        bool SaveCreatedPackage(PackageDefinition definition);

        /// <summary>
        /// Creates the package file and returns it's physical path
        /// </summary>
        /// <param name="definition"></param>
        string ExportCreatedPackage(PackageDefinition definition);

    }
}
