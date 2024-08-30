using System.Collections.Generic;
using Newtonsoft.Json;
using Samurai.Application;
using Samurai.Application.Saving;
using Samurai.Example.Defs;

namespace Samurai.Example;

public class LevelModel : ISavable
{
    [JsonIgnore]
    public string Id => "Level";

    public readonly string LevelId;
    public readonly List<string> SpawnedWaveIds = new();

    public LevelModel(string levelId)
    {
        LevelId = levelId;
        if (Definitions.Get<LevelDefinition>(LevelId) is null)
        {
            Log.Error($"LevelDefinition with id '{levelId}' does not exist!", nameof(LevelModel));
        }
    }
}