using System;
using System.Runtime.InteropServices;
namespace server.Classes.Constants
{
    public class ConfigRuntimeConstants
    {
        public static readonly string FFMPEG = "ffmpeg";
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