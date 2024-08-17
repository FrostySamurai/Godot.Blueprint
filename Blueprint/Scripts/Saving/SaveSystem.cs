using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Newtonsoft.Json;
using Samurai.Application.Configs;

namespace Samurai.Application.Saving
{
    public class SaveSystem
    {
        public const string Autosave = "Autosave";
        internal const string LogTag = "Saves";
        private const string SaveExtension = ".save";
        
        private readonly string _saveFolder;
        private readonly List<SessionSaves> _saves = new();

        public IReadOnlyList<SessionSaves> Saves => _saves;

        // TODO: not the best way to do this
        internal static JsonSerializerSettings SerializerSettings { get; } = new()
        {
            TypeNameHandling = TypeNameHandling.All, // TODO: maybe not all?
        };
        
        #region Lifecycle

        public SaveSystem()
        {
            var config = Definitions.Config<AppConfig>();
            _saveFolder = Path.Combine(OS.GetUserDataDir(), config.SavesFolder);
            Log.Debug($"Save folder on path: {_saveFolder}", LogTag);
        
            if (!Directory.Exists(_saveFolder))
            {
                Directory.CreateDirectory(_saveFolder);
            }

            var saves = new List<string>();
            foreach (string directory in Directory.EnumerateDirectories(_saveFolder))
            {
                string path = Path.Combine(_saveFolder, directory);
                
                var files = Directory.EnumerateFiles(path).OrderByDescending(File.GetLastWriteTime);
                foreach (string filePath in files)
                {
                    if (filePath is null || !filePath.EndsWith(SaveExtension))
                    {
                        continue;
                    }
                
                    saves.Add(Path.GetFileNameWithoutExtension(filePath));
                }
                
                string sessionId = Path.GetFileNameWithoutExtension(directory);
                _saves.Add(new SessionSaves(sessionId, saves));
                saves.Clear();
            }
        }
        
        #endregion Lifecycle

        #region Queries

        public bool TryGetSaves(string sessionId, out SessionSaves saves)
        {
            saves = _saves.FirstOrDefault(x => x.SessionId == sessionId);
            return saves is not null;
        }

        public bool Has(string sessionId, string fileName)
        {
            return _saves.FirstOrDefault(x => x.SessionId == sessionId)?.SaveFiles.Contains(fileName) ?? false;
        }

        #endregion Queries
        
        #region Public
        
        public T Load<T>(string sessionId, string fileName)
        {
            Log.Debug($"Loading '{fileName}' for session '{sessionId}'.", LogTag);
            
            string path = GetSavePath(sessionId, fileName);
            if (!File.Exists(path))
            {
                Log.Debug($"Save file '{fileName}' for session '{sessionId}' doesn't exist.", LogTag);
                return default;
            }

            using var file = File.OpenRead(path);
            using var reader = new BinaryReader(file);
            string content = reader.ReadString();
            var state = JsonConvert.DeserializeObject<T>(content, SerializerSettings);
            Log.Debug($"Loaded '{fileName}' for session '{sessionId}'.", LogTag);
            return state;
        }
        
        public void Save<T>(string sessionId, string fileName, T state)
        {
            Log.Debug($"Saving '{fileName}' for session '{sessionId}'.", LogTag);

            if (!typeof(T).IsSerializable)
            {
                Log.Error($"Can't save session. '{typeof(T).Name}' is not serializable.", LogTag);
                return;
            }
            
            string sessionFolder = Path.Combine(_saveFolder, sessionId);
            if (!Directory.Exists(sessionFolder))
            {
                Directory.CreateDirectory(sessionFolder);
            }
            
            string path = GetSavePath(sessionId, fileName);
            using var file = File.Open(path, FileMode.Create);
            using var writer = new BinaryWriter(file);
            writer.Write(JsonConvert.SerializeObject(state, SerializerSettings));
            Log.Debug($"Saved '{fileName}' for session '{sessionId}'.", LogTag);

            if (!TryGetSaves(sessionId, out var saves))
            {
                saves = new SessionSaves(sessionId);
                _saves.Add(saves);
            }
            
            saves.Add(fileName);
        }

        public void Delete(string sessionId)
        {
            string sessionFolder = Path.Combine(_saveFolder, sessionId);
            if (Directory.Exists(sessionFolder))
            {
                Directory.Delete(sessionFolder,true);
                _saves.RemoveAll(x => x.SessionId == sessionId);
            }
        }

        public void Delete(string sessionId, string fileName)
        {
            string sessionFolder = Path.Combine(_saveFolder, sessionId);
            if (!Directory.Exists(sessionFolder))
            {
                return;
            }
            
            string path = GetSavePath(sessionId, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
                _saves.FirstOrDefault(x => x.SessionId == sessionId)?.Remove(fileName);
            }

            if (!Directory.EnumerateFileSystemEntries(sessionFolder).Any())
            {
                Directory.Delete(sessionFolder);
                _saves.RemoveAll(x => x.SessionId == sessionId);
            }
        }
        
        #endregion Public
        
        #region Private
        
        private string GetSavePath(string sessionId, string fileName)
        {
            string path = Path.Combine(_saveFolder, sessionId, fileName);
            return $"{path}{SaveExtension}";
        }
        
        #endregion Private
    }
}