// Copyright (c) Sbn.
// See LICENSE for more details.

using System.Collections.Generic;

namespace Sbn.Cms.Core.Install
{
    /// <summary>
    /// Helper to test File and folder permissions
    /// </summary>
    public interface IFilePermissionHelper
    {
        /// <summary>
        /// Run all tests for permissions of the required files and folders.
        /// </summary>
        /// <returns>True if all permissions are correct. False otherwise.</returns>
        bool RunFilePermissionTestSuite(out Dictionary<FilePermissionTest, IEnumerable<string>> report);

    }
}
