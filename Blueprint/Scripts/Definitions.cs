using System;
using System.Collections.Generic;
using System.Linq;
using Samurai.Application.Utility;

namespace Samurai.Application
{
    
    public class Definitions
    {
        internal const string LogTag = "Definitions";

        #region Static

        private static Definitions _instance;

        internal static void Create(string resourcesPath, string definitionsFolder, string configsFolder)
        {
            if (_instance != null)
            {
                Log.Debug("An instance of definitions is already created. Skipping creation..", LogTag);
                return;
            }

            _instance = new Definitions(resourcesPath, definitionsFolder, configsFolder);
            Log.Debug("Initialized.", LogTag);
        }

        internal static void Dispose()
        {
            _instance = null;
            Log.Debug("Disposed.", LogTag);
        }

        public static T Config<T>() where T : Config
        {
            return _instance._configs.TryGetValue(typeof(T), out var config) ? (T)config : null;
        }

        public static bool TryGet<T>(string id, out T definition) where T : Definition
        {
            definition = Get<T>(id);
            return definition is not null;
        }
        
        public static T Get<T>(string id) where T : Definition
        {
            if (!_instance._definitions.TryGetValue(typeof(T), out var definitions))
            {
                return null;
            }

            return definitions.TryGetValue(id, out var definition) ? (T)definition : null;
        }

        public static IEnumerable<T> Get<T>(Predicate<T> predicate = null) where T : Definition
        {
            if (!_instance._definitions.TryGetValue(typeof(T), out var definitions))
            {
                return Enumerable.Empty<T>();
            }

            var typed = definitions.Values.Cast<T>();
            return predicate != null ? typed.Where(x => predicate(x)) : typed;
        }

        public static void Get<T>(List<T> result, Predicate<T> predicate = null) where T : Definition
        {
            if (!_instance._definitions.TryGetValue(typeof(T), out var definitions))
            {
                return;
            }

            var typed = definitions.Values.Cast<T>();
            if (predicate == null)
            {
                result.AddRange(typed);
                return;
            }

            result.AddRange(typed.Where(x => predicate(x)));
        }

        #endregion Static
        
        private readonly Dictionary<Type, Dictionary<string, Definition>> _definitions = new();
        private readonly Dictionary<Type, Config> _configs = new();

        private Definitions(string resourcesPath, string definitionsFolder, string configsFolder)
        {
            var definitions = new List<Definition>();
            FileUtils.LoadResources($"{resourcesPath}/{definitionsFolder}", definitions, FileUtils.ResourceExtension);
            var grouped = definitions.GroupBy(x => x.GetType());
            foreach (var group in grouped)
            {
                var dict = new Dictionary<string, Definition>();
                foreach (var entry in group)
                {
                    if (!dict.TryAdd(entry.Id, entry))
                    {
                        Log.Error($"Duplicate id '{entry.Id}' for definition type '{group.Key.Name}'.", LogTag);
                    }
                }

                _definitions[group.Key] = dict;
            }

            var configs = new List<Config>();
            FileUtils.LoadResources($"{resourcesPath}/{configsFolder}", configs, FileUtils.ResourceExtension);
            foreach (var config in configs)
            {
                if (!_configs.TryAdd(config.GetType(), config))
                {
                    Log.Error($"Duplicate settings of type '{_configs.GetType().Name}'.", LogTag);
                }
            }
        }
    }
}