namespace Sbn.Cms.Core
{
    /// <summary>
    /// Describes the levels in which the runtime can run.
    /// </summary>
    public enum RuntimeLevel
    {
        /// <summary>
        /// The runtime has failed to boot and cannot run.
        /// </summary>
        BootFailed = -1,

        /// <summary>
        /// The level is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The runtime is booting.
        /// </summary>
        Boot = 1,

        /// <summary>
        /// The runtime has detected that Sbn is not installed at all, ie there is
        /// no database, and is currently installing Sbn.
        /// </summary>
        Install = 2,

        /// <summary>
        /// The runtime has detected an Sbn install which needed to be upgraded, and
        /// is currently upgrading Sbn.
        /// </summary>
        Upgrade = 3,

        /// <summary>
        /// The runtime has detected an up-to-date Sbn install and is running.
        /// </summary>
        Run = 100
    }
}
