using System.Collections.Generic;

namespace Samurai.Application.Saving
{
    public class SessionSaves
    {
        private readonly List<string> _saveFiles;
        
        public readonly string SessionId;
        public IReadOnlyList<string> SaveFiles => _saveFiles;

        public SessionSaves(string sessionId)
        {
            SessionId = sessionId;
            _saveFiles = new List<string>();
        }
        
        public SessionSaves(string sessionId, IEnumerable<string> saves)
        {
            SessionId = sessionId;
            _saveFiles = new List<string>(saves);
        }

        public void Add(string save)
        {
            if (!_saveFiles.Contains(save))
            {
                _saveFiles.Add(save);
            }
        }

        public void Remove(string save)
        {
            _saveFiles.Remove(save);
        }
    }
}