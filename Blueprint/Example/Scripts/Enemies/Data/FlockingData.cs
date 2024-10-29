using System;
using System.Collections.Generic;
using Godot;
using Newtonsoft.Json;
using Samurai.Application;
using Samurai.Example.Enemies.Defs;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies.Data;

[Serializable]
public class FlockingData : IComponentData
{
    [JsonIgnore]
    public bool IsDebug;
    [JsonIgnore]
    public FlockingComponentDefinition Definition;
    [JsonIgnore]
    public HashSet<Enemy> Flock = new();
    [JsonIgnore]
    public Vector2 FlockCenter;
    
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