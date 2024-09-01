using System;
using Newtonsoft.Json;
using Samurai.Application;
using Samurai.Example.Entities.Health.Defs;

namespace Samurai.Example.Entities.Health.Data;

[Serializable]
public class HealthData : IComponentData
{
    [JsonIgnore]
    public HealthDefinition Definition;
    
    public string DefinitionId;
    public int Current;

    public void OnLoad()
    {
        Definition = Definitions.Get<HealthDefinition>(DefinitionId);
    }
}