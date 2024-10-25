using System.Collections.Generic;
using Samurai.Application;
using Samurai.Application.Saving;
using Samurai.Example.Defs;

namespace Samurai.Example;

public class LevelModel : ISavable
{
    public readonly string LevelId;
    public readonly HashSet<string> SpawnedWaveIds = new();

    public LevelModel(string levelId)
    {
        LevelId = levelId;
        if (Definitions.Get<LevelDefinition>(LevelId) is null)
        {
            Log.Error($"LevelDefinition with id '{levelId}' does not exist!", nameof(LevelModel));
        }
    }
}