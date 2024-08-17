using Godot;

namespace Samurai.Application.SessionHandling
{
    public partial class NodeSessionHandler : Node, ISessionHandler
    {
        [Export]
        private int _priority;

        public int Priority => _priority;

        public virtual void OnSessionStart()
        {
            
        }

        public virtual void OnSessionEnd()
        {
            
        }
    }
}