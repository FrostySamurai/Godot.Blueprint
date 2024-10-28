using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Health;

namespace Samurai.Example.Entities;

public partial class Entity : Node2D
{
    [Export]
    private Node2D _root;

    public string Id { get; internal set; }

    public override void _EnterTree()
    {
        base._EnterTree();
        
        Session.Events.Register<HealthEvents.OnDestroyed>(OnDestroyed, this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        Session.Events.Unregister<HealthEvents.OnDestroyed>(this);
    }

    private void OnDestroyed(HealthEvents.OnDestroyed evt)
    {
        if (evt.IsMatch(Id))
        {
            CallDeferred(nameof(Return));
        }
    }

    private void Return()
    {
        NodePool.Return(_root);
    }
}