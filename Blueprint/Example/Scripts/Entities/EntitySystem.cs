using System;
using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;

namespace Samurai.Example.Entities;

public static class EntitySystem
{
    public const string LogTag = "Entities";

    public static Entity Spawn(EntityDefinition definition, Node2D parent, Vector2 position)
    {
        var instance = NodePool.RetrieveParentless<Entity>(definition.Prefab);
        if (instance is null)
        {
            Log.Error($"Failed to instantiate entity '{definition.Id}'! Error in prefab..", LogTag);
            return null;
        }

        instance.GlobalPosition = position;

        string id = Guid.NewGuid().ToString();
        instance.Id = id;

        var data = new EntityData(id, definition, instance);
        var model = Session.Get<EntityModel>();
        model.Add(data);
        
        parent.AddChild(instance);
        
        Session.Events.Raise(new EntityEvents.OnEntitySpawned(id));

        return instance;
    }
}