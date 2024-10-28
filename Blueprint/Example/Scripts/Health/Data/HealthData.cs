using System;
using Newtonsoft.Json;
using Samurai.Application;
using Samurai.Example.Entities;
using Samurai.Example.Health.Defs;

namespace Samurai.Example.Health.Data;

[Serializable]
public class HealthData : IComponentData
{
    [JsonIgnore]
    public HealthComponentDefinition Definition;
    
    public string DefinitionId;
    public int Current;

    public HealthData(HealthComponentDefinition definition)
    {
        Definition = definition;
        DefinitionId = definition.Id;
        Current = Definition.MaxHealth;
    }

    public void OnLoad()
    {
        Definition = Definitions.Get<HealthComponentDefinition>(DefinitionId);
    }
}