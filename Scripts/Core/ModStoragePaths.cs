using System.IO;
using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    internal static class ModStoragePaths
    {
        internal const string ModId = "LIB_BaPlayerLocation";
        internal const string ConfigFileName = "subscriber_config.json";

        internal static string ModRootDirectory
        {
            get
            {
                var path = Path.Combine(Application.persistentDataPath, "ModsLocal", ModId);
                Directory.CreateDirectory(path);
                return path;
            }
        }

        internal static string ConfigFilePath => Path.Combine(ModRootDirectory, ConfigFileName);
    }
}
