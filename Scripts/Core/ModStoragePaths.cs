using System;
using System.IO;
using BAModAPI;
using UnityEngine;

namespace BaPlayerLocation.Subscriber
{
    /// <summary>
    /// All mod files live under <see cref="ModContext.ModRootPath"/> only.
    /// </summary>
    internal static class ModStoragePaths
    {
        internal const string ModId = "LIB_BaPlayerLocation";
        internal const string ModsLocalFolder = "ModsLocal";
        internal const string ConfigFileName = "subscriber_config.json";
        internal const string ShippedConfigExampleFileName = "subscriber_config.json.example";

        private static string _modRoot;

        internal static string ModRootDirectory
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_modRoot))
                    return _modRoot;

                return FallbackModsLocalRoot();
            }
        }

        internal static void Initialize(ModContext context)
        {
            _modRoot = string.IsNullOrWhiteSpace(context?.ModRootPath)
                ? null
                : context.ModRootPath;
        }

        internal static void Shutdown() => _modRoot = null;

        internal static string PathInModRoot(string relativePath) =>
            CombineRelative(ModRootDirectory, relativePath);

        internal static string ConfigFilePath =>
            PathInModRoot(ConfigFileName);

        private static string FallbackModsLocalRoot()
        {
            var path = Path.Combine(Application.persistentDataPath, ModsLocalFolder, ModId);
            Directory.CreateDirectory(path);
            return path;
        }

        private static string CombineRelative(string root, string relativePath)
        {
            if (Path.IsPathRooted(relativePath))
                throw new ArgumentException("Path must be relative to the mod root.", nameof(relativePath));

            return Path.Combine(root, relativePath);
        }
    }
}
