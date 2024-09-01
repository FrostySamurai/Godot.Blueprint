using System;
using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Entities.Health;

namespace Samurai.Example.Entities;

public partial class Entity : Node2D
{
    [Export]
    private Node2D _root;
    
    public string Id { get; private set; } = null;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        Id ??= Guid.NewGuid().ToString();
        Session.Get<EntityModel>().Register(this);
        Session.Events.Register<HealthEvents.OnDestroyed>(OnDestroyed, this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        Session.Get<EntityModel>().Unregister(Id);
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