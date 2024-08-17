using System;
using System.Collections.Generic;
using System.Text.Json;
using Godot;
using RedHerring.Extensions.Collections;

namespace Samurai.Application.Utility
{
    public static class FileUtils
    {
        public const string ResourceExtension = "tres";
        
        /// <summary>
        /// Fills the passed array with all absolute file paths contained within the root folder recursively.
        /// </summary>
        public static void GetFiles(string rootFolderPath, List<string> filePaths, string suffix = null)
        {
            if (!DirAccess.DirExistsAbsolute(rootFolderPath))
            {
                Log.Error($"Folder to load resources from doesn't exist. Path: {rootFolderPath}", string.Empty);
                return;
            }
            
            var directories = new List<string> { rootFolderPath };
            while (directories.NotNullOrEmpty())
            {
                string path = directories[0];
                directories.RemoveAt(0);
                if (!DirAccess.DirExistsAbsolute(path))
                {
                    continue;
                }

                // TODO: possibly optimize
                var directory = DirAccess.Open(path);
                string[] subdirectories = directory.GetDirectories();
                Array.ForEach(subdirectories,x => directories.Add($"{path}/{x}"));
                foreach (string fileName in directory.GetFiles())
                {
                    if (suffix.NotNullOrEmpty() && !fileName.EndsWith(suffix))
                    {
                        continue;
                    }
                    
                    filePaths.Add($"{path}/{fileName}");
                }
            }
        }

        /// <summary>
        /// Loads all resources of given type from passed folder recusively.
        /// </summary>
        public static void LoadResources<T>(string rootFolderPath, List<T> resources, string suffix = null) where T : Resource
        {
            var filePaths = new List<string>();
            GetFiles(rootFolderPath, filePaths, suffix);
            LoadResources(filePaths, resources);
        }

        /// <summary>
        /// Loads all resources of given type from paths passed in the parameters.
        /// </summary>
        public static void LoadResources<T>(IEnumerable<string> files, List<T> resources) where T : Resource
        {
            foreach (string filePath in files)
            {
                try
                {
                    var resource = ResourceLoader.Load<T>(filePath);
                    if (resource is not null)
                    {
                        resources.Add(resource);
                    }
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to load resource on path: {filePath}. Reason: {e.Message}", string.Empty);
                    continue;
                }
            }
        }

        public static bool TryDeserialize<T>(string json, out T deserialized)
        {
            deserialized = default;
            try
            {
                deserialized = JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception e)
            {
                Log.Error($"Failed to deserialize. Reason: {e.Message}.", string.Empty);
                return false;
            }

            return true;
        }
    }
}