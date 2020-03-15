using System;
using System.Runtime.InteropServices;
namespace server.Classes.Constants
{
    public class ConfigRuntimeConstants
    {
        public static readonly string FFMPEG = "ffmpeg";
        public static readonly string GLOBAL_DEFAULTS = "global_defaults";
        public static readonly string SPECIFIC_DEFAULTS = "specific_defaults";
        public static readonly string PARALOGUE = "paralogue";
        public static readonly string EPILOGUE = "epilogue";
        public static readonly string NETWORK = "network";
        public static readonly string SETTINGS_FILE = "appsettings.json";
        public static string OS
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return "windows";
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return "osx";
                }
                else
                {
                    return "linux";
                }
            }
        }
    }
}