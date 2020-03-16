using System.Runtime.InteropServices;
namespace client.Classes.Constants
{
    public static class ConfigRuntimeConstants
    {
        public static readonly string MPV = "mpv";
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
    public enum Arguments
    {
        f,
    }
}