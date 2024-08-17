using Godot;

namespace Samurai.Application
{
    public abstract partial class Definition : Resource
    {
        [Export]
        public string Id;
    }
}