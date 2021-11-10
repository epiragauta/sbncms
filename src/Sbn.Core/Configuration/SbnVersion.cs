using System;
using System.Reflection;
using Sbn.Cms.Core.Semver;
using Sbn.Extensions;

namespace Sbn.Cms.Core.Configuration
{
    /// <summary>
    /// Represents the version of the executing code.
    /// </summary>
    public class SbnVersion : ISbnVersion
    {
        public SbnVersion()
        {
            var sbnCoreAssembly = typeof(SemVersion).Assembly;

            // gets the value indicated by the AssemblyVersion attribute
            AssemblyVersion = sbnCoreAssembly.GetName().Version;

            // gets the value indicated by the AssemblyFileVersion attribute
            AssemblyFileVersion = System.Version.Parse(sbnCoreAssembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version);

            // gets the value indicated by the AssemblyInformationalVersion attribute
            // this is the true semantic version of the Sbn Cms
            SemanticVersion = SemVersion.Parse(sbnCoreAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion);

            // gets the non-semantic version
            Version = SemanticVersion.GetVersion(3);
        }

        /// <summary>
        /// Gets the non-semantic version of the Sbn code.
        /// </summary>
        public Version Version { get; }

        /// <summary>
        /// Gets the semantic version comments of the Sbn code.
        /// </summary>
        public string Comment => SemanticVersion.Prerelease;

        /// <summary>
        /// Gets the assembly version of the Sbn code.
        /// </summary>
        /// <remarks>
        /// <para>The assembly version is the value of the <see cref="AssemblyVersionAttribute"/>.</para>
        /// <para>Is the one that the CLR checks for compatibility. Therefore, it changes only on
        /// hard-breaking changes (for instance, on new major versions).</para>
        /// </remarks>
        public Version AssemblyVersion { get; }

        /// <summary>
        /// Gets the assembly file version of the Sbn code.
        /// </summary>
        /// <remarks>
        /// <para>The assembly version is the value of the <see cref="AssemblyFileVersionAttribute"/>.</para>
        /// </remarks>
        public Version AssemblyFileVersion { get; }

        /// <summary>
        /// Gets the semantic version of the Sbn code.
        /// </summary>
        /// <remarks>
        /// <para>The semantic version is the value of the <see cref="AssemblyInformationalVersionAttribute"/>.</para>
        /// <para>It is the full version of Sbn, including comments.</para>
        /// </remarks>
        public SemVersion SemanticVersion { get; }
    }
}
