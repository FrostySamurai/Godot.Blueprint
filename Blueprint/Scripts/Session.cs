using System;
using System.Collections.Generic;
using System.Linq;
using Samurai.Application.Configs;
using Samurai.Application.Events;
using Samurai.Application.Saving;
using Samurai.Application.SessionHandling;

namespace Samurai.Application
{
    public class Session
    {
        internal const string LogTag = "Session";
        
        #region Static

        private static Session _instance;

        public static EventAggregator Events => Get<EventAggregator>();

        private static readonly List<ISessionHandler> SessionHandlers = new();

        #region Lifecycle

        public static void Start(string sessionId, string saveName)
        {
            _instance = new Session(sessionId, saveName);
            SessionHandlers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            SessionHandlers.ForEach(x => x.OnSessionStart());
            
            Log.Debug($"Session '{sessionId}' started.", LogTag);
        }

        public static void BeforeSessionUnload()
        {
            bool isAutosaveEnabled = Definitions.Config<AppConfig>().EnableAutosaves;
            if (isAutosaveEnabled)
            {
                // TODO: also this should be handled in custom handlers probably
                // TODO: do separate function for auto save, support for multiple saves
                Save(SaveSystem.Autosave);
            }
        }

        public static void End()
        {
            string sessionId = _instance._sessionId;
            
            SessionHandlers.Sort((x, y) => x.Priority.CompareTo(y.Priority));
            SessionHandlers.ForEach(x => x.OnSessionEnd());
            
            _instance.Dispose();
            _instance = null;
            
            Log.Debug($"Session '{sessionId}' ended.", LogTag);
        }

        public static void Register(IEnumerable<ISessionHandler> sessionHandlers)
        {
            foreach (var entry in sessionHandlers)
            {
                if (SessionHandlers.Contains(entry))
                {
                    continue;
                }
                
                SessionHandlers.Add(entry);
            }
        }

        public static void Register(ISessionHandler sessionHandler)
        {
            if (!SessionHandlers.Contains(sessionHandler))
            {
                SessionHandlers.Add(sessionHandler);   
            }
            
        }

        public static void Unregister(IEnumerable<ISessionHandler> sessionHandlers)
        {
            foreach (var entry in sessionHandlers)
            {
                SessionHandlers.Remove(entry);
            }
        }

        public static void Unregister(ISessionHandler sessionHandler)
        {
            SessionHandlers.Remove(sessionHandler);
        }

        #endregion Lifecycle

        #region Access

        public static bool Exists()
        {
            return _instance is not null;
        }

        public static void Add<T>(T obj, bool overwrite = false)
        {
            if (!Exists())
            {
                return;
            }

            var type = typeof(T);
            if (!overwrite && _instance._content.ContainsKey(type))
            {
                return;
            }
            
            _instance._content[type] = obj;
        }

        public static T Get<T>()
        {
            if (!Exists())
            {
                return default;
            }
            
            if (_instance._content.TryGetValue(typeof(T), out var obj))
            {
                return (T)obj;
            }

            return App.Get<T>();
        }

        #endregion Access

        #region Saves

        public static void Save(string fileName)
        {
            Get<SaveSystem>().Save(_instance._sessionId, fileName, _instance.GetSaveState());
        }

        #endregion Saves

        #endregion Static

        private readonly Dictionary<Type, object> _content = new();

        private readonly string _sessionId;

        private Session(string sessionId, string saveName)
        {
            _sessionId = sessionId;
            Add(new EventAggregator(App.Get<EventAggregator>()));
            
            if (!string.IsNullOrEmpty(saveName))
            {
                var save = App.Get<SaveSystem>().Load<List<object>>(sessionId, saveName);
                save?.ForEach(x => _content[x.GetType()] = x);
            }
        }

        private List<object> GetSaveState()
        {
            return new List<object>(_content.Values.Where(x => x is ISavable));
        }

        private void Dispose()
        {
            foreach (object entry in _content.Values)
            {
                if (entry is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _content.Clear();
        }
    }
}
