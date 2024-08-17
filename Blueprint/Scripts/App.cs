using System;
using System.Collections.Generic;
using Godot;
using Samurai.Application.Configs;
using Samurai.Application.Events;
using Samurai.Application.Saving;
using Samurai.Application.Scenes;

namespace Samurai.Application
{
    public static class App
    {
        internal static string LogTag = "Application";
        
        private static Dictionary<Type, object> _content = new();
        private static SceneTree _sceneTree;

        public static bool IsPaused => _sceneTree.Paused;
        public static EventAggregator Events => Get<EventAggregator>();

        internal static void Init(SceneTree root)
        {
            _sceneTree = root;
            Log.Debug("Initializing.", LogTag);

            Add(new SaveSystem());
            Add(new EventAggregator());

            Log.Debug("Initialized.", LogTag);
        }
        
        public static void Add<T>(T obj)
        {
            _content[typeof(T)] = obj;
        }

        public static T Get<T>()
        {
            return _content.TryGetValue(typeof(T), out var obj) ? (T)obj : default;
        }

        public static void StartSession(string sessionId, string saveName = null)
        {
            Log.Debug("Starting session.", LogTag);
            
            // TODO: make this async?
            Session.Start(sessionId, saveName);

            var config = Definitions.Config<AppConfig>();
            SceneLoader.UnloadScene(config.MainMenuScene);
            SceneLoader.LoadScene(config.SessionScene);
            Log.Debug("Session started.", LogTag);
        }

        public static void EndSession()
        {
            Log.Debug("Ending session.", LogTag);
            Session.BeforeSessionUnload();
            
            var config = Definitions.Config<AppConfig>();
            SceneLoader.UnloadScene(config.SessionScene);
            SceneLoader.LoadScene(config.MainMenuScene);
            
            Session.End();
            SetPause(false);
            
            Log.Debug("Session ended.", LogTag);
        }

        public static void SetPause(bool state)
        {
            if (IsPaused == state)
            {
                return;
            }
            
            _sceneTree.Paused = state;
            Log.Debug($"Pause = {state}", nameof(App));
        }

        public static void TogglePause()
        {
            SetPause(!IsPaused);
        }
        
        public static void Quit()
        {
            if (Session.Exists())
            {
                Session.BeforeSessionUnload();
                Session.End();
            }
            
            Definitions.Dispose();
            _sceneTree.Quit();
        }
    }
}
