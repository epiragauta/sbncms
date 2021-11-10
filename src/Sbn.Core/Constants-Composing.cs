namespace Sbn.Cms.Core
{
    /// <summary>
    /// Defines constants.
    /// </summary>
    public static partial class Constants
    {
        /// <summary>
        /// Defines constants for composition.
        /// </summary>
        public static class Composing
        {
            public static readonly string[] SbnCoreAssemblyNames = new[]
            {
                "Sbn.Core",
                "Sbn.Infrastructure",
                "Sbn.PublishedCache.NuCache",
                "Sbn.Examine.Lucene",
                "Sbn.Web.Common",
                "Sbn.Web.BackOffice",
                "Sbn.Web.Website",
            };
        }
    }
}
