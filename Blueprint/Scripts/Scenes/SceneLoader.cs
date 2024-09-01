using System;
using System.Collections.Generic;
using Godot;
using Samurai.Application.Utility;

namespace Samurai.Application.Scenes
{
    public static class SceneLoader
    {
        private const string LogTag = nameof(SceneLoader);

        private static Node _parent;
        
        private static readonly Dictionary<PackedScene, Node> _loadedScenes = new();

        #region Lifecycle

        public static void Init(Node parent)
        {
            if (_parent is null)
            {
                _parent = parent;
                Log.Debug("Initialized.", LogTag);
            }
        }

        #endregion Lifecycle

        #region Public

        public static void LoadScene(PackedScene scene)
        {
            if (scene is null)
            {
                return;
            }

            string sceneName = scene.Name();
            if (_parent is null)
            {
                Log.Error($"Cannot load scene '{sceneName}'. SceneLoader was not initialized!", LogTag);
                return;
            }

            Log.Debug($"Loading scene '{sceneName}'.", LogTag);
            
            if (_loadedScenes.TryGetValue(scene, out var loaded))
            {
                Log.Debug($"Previous instance of scene found. Disposing..", LogTag);
                loaded.QueueFree();
            }

            loaded = scene.Instantiate();
            _parent.AddChild(loaded);
            
            _loadedScenes[scene] = loaded;
            
            Log.Debug($"Scene '{sceneName}' loaded.", LogTag);
        }

        public static void UnloadScene(PackedScene scene, Action onUnloaded = null)
        {
            if (scene is null)
            {
                return;
            }

            string sceneName = scene.Name();
            if (_parent is null)
            {
                Log.Error($"Cannot unload scene '{sceneName}'. SceneLoader was not initialized!", LogTag);
                return;
            }

            Log.Debug($"Unloading scene '{sceneName}'.", nameof(SceneLoader));
            if (!_loadedScenes.TryGetValue(scene, out var loaded))
            {
                return;
            }

            loaded.QueueFree();
            _loadedScenes.Remove(scene);
            loaded.TreeExited += () =>
            {
                onUnloaded?.Invoke();
                Log.Debug($"Scene '{sceneName}' unloaded.", nameof(SceneLoader));
            };
        }

        #endregion Public
    }
}