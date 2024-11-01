using System.Collections.Generic;
using Godot;
using Samurai.Application;
using Samurai.Example.Enemies.Defs;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies.Data;

public class FlockingData : IComponentData
{
    public EntityComponentDefinition ComponentDefinition => Definition;

    public bool IsDebug;
    public Enemy Self;
    public FlockingComponentDefinition Definition;
    public HashSet<Enemy> Flock = new();

    public Vector2 Center;
    public Vector2 Alignment;
    public Vector2 Separation;
    
    public string DefinitionId;

    public FlockingData(FlockingComponentDefinition definition)
    {
        Definition = definition;
        DefinitionId = definition.Id;
    }

    public void OnLoad()
    {
        Definition = Definitions.Get<FlockingComponentDefinition>(DefinitionId);
    }
}