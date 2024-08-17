using System.Collections.Generic;
using Godot;

namespace Samurai.Application.Scenes
{
    public static class SceneLoader
    {
        private static readonly Dictionary<PackedScene, Node> _loadedScenes = new();
        
        public static void LoadScene(PackedScene scene)
        {
            if (scene is null)
            {
                return;
            }
            
            Log.Debug($"Loading scene '{scene.GetPath()}'.", nameof(SceneLoader));
            
            var sceneParent = App.Get<SceneParent>();
            if (sceneParent is null)
            {
                Log.Error($"{nameof(SceneParent)} is missing from the hierarchy! Can't load scene.", nameof(SceneLoader));
                return;
            }

            if (_loadedScenes.TryGetValue(scene, out var loaded))
            {
                Log.Debug($"Previous instance of scene found. Disposing..", nameof(SceneLoader));
                loaded.QueueFree();
            }

            loaded = scene.Instantiate();
            sceneParent.AddChild(loaded);
            
            _loadedScenes[scene] = loaded;
            
            Log.Debug($"Scene '{scene.GetPath()}' loaded.", nameof(SceneLoader));
        }

        public static void UnloadScene(PackedScene scene)
        {
            if (scene is null)
            {
                return;
            }

            Log.Debug($"Unloading scene '{scene.GetPath()}'.", nameof(SceneLoader));
            if (!_loadedScenes.TryGetValue(scene, out var loaded))
            {
                return;
            }

            loaded.QueueFree();
            _loadedScenes.Remove(scene);
            Log.Debug($"Scene '{scene.GetPath()}' unloaded.", nameof(SceneLoader));
        }
    }
}