// Copyright (c) Sbn.
// See LICENSE for more details.

using Sbn.Cms.Core.Semver;

namespace Sbn.Extensions
{
    public static class SemVersionExtensions
    {
        public static string ToSemanticString(this SemVersion semVersion)
        {
            return semVersion.ToString().Replace("--", "-").Replace("-+", "+");
        }

        public static string ToSemanticStringWithoutBuild(this SemVersion semVersion)
        {
            var version = semVersion.ToSemanticString();
            var indexOfBuild = version.IndexOf('+');
            return indexOfBuild >= 0 ? version.Substring(0, indexOfBuild) : version;
        }
    }
}
