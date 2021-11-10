// Copyright (c) Sbn.
// See LICENSE for more details.

namespace Sbn.Cms.Core.Configuration.Models
{
    public enum LuceneDirectoryFactory
    {
        /// <summary>
        /// The index will operate from the default location: Sbn/Data/Temp/ExamineIndexes
        /// </summary>
        Default,

        /// <summary>
        /// The index will operate on a local index created in the processes %temp% location and
        /// will replicate back to main storage in Sbn/Data/Temp/ExamineIndexes
        /// </summary>
        SyncedTempFileSystemDirectoryFactory,

        /// <summary>
        /// The index will operate only in the processes %temp% directory location
        /// </summary>
        TempFileSystemDirectoryFactory
    }
}
