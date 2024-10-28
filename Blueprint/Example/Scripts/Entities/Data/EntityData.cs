using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Samurai.Application;

namespace Samurai.Example.Entities;

[Serializable]
public class EntityData
{
    public string DefinitionId;
    public Dictionary<Type, IComponentData> Components = new();

    [JsonIgnore]
    public EntityDefinition Definition;
    [JsonIgnore]
    public Entity Root;

    public EntityData(EntityDefinition definition, Entity entity)
    {
        Definition = definition;
        DefinitionId = definition.Id;
        Root = entity;

        foreach (var entry in Definition.Components)
        {
            var data = entry.Create();
            if (!Components.TryAdd(data.GetType(), data))
            {
                Log.Error($"Duplicate component definition of type '{entry.GetType().Name}' on entity '{Definition.Id}'! Skipping..", EntitySystem.LogTag);
            }
        }
    }

    public void OnSave()
    {
        foreach (var entry in Components.Values)
        {
            entry.OnSave();
        }
    }

    public void OnLoad()
    {
        foreach (var entry in Components.Values)
        {
            entry.OnLoad();
        }
    }

    // TODO: save common stuff like position, rotation so that it can be loaded
}