namespace Sbn.Cms.Core
{
    public static partial class Constants
    {
        public static class SystemDirectories
        {
            /// <summary>
            /// The aspnet bin folder
            /// </summary>
            public const string Bin = "~/bin";

            // TODO: Shouldn't this exist underneath /Sbn in the content root?
            public const string Config = "~/config";

            /// <summary>
            /// The Sbn folder that exists at the content root.
            /// </summary>
            /// <remarks>
            /// This is not the same as the Sbn web folder which is configurable for serving front-end files.
            /// </remarks>
            public const string Sbn = "~/sbn";

            /// <summary>
            /// The Sbn data folder in the content root.
            /// </summary>
            public const string Data = Sbn + "/Data";

            /// <summary>
            /// The Sbn licenses folder in the content root.
            /// </summary>
            public const string Licenses = Sbn + "/Licenses";

            /// <summary>
            /// The Sbn temp data folder in the content root.
            /// </summary>
            public const string TempData = Data + "/TEMP";

            public const string TempFileUploads = TempData + "/FileUploads";

            public const string TempImageUploads = TempFileUploads + "/rte";

            public const string Install = "~/install";

            public const string AppPlugins = "/App_Plugins";
            public static string AppPluginIcons => "/Backoffice/Icons";

            public const string MvcViews = "~/Views";

            public const string PartialViews = MvcViews + "/Partials/";

            public const string MacroPartials = MvcViews + "/MacroPartials/";

            public const string Packages = Data + "/packages";

            public const string Preview = Data + "/preview";

            /// <summary>
            /// The default folder where Sbn log files are stored
            /// </summary>
            public const string LogFiles = Sbn + "/Logs";
        }
    }
}
