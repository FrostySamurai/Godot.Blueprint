using System.Collections.Generic;
using Godot;
using RedHerring.Extensions.Collections;
using Samurai.Application;
using Samurai.Example.Defs;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Enemies.Defs;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies;

public partial class Spawner : Node2D
{
    private readonly List<WaveDefinition> _waves = new();

    private RandomNumberGenerator _rng = new();
    
    public override void _EnterTree()
    {
        Session.Add(this);

        var levelModel = Session.Get<LevelModel>();
        var levelDef = Definitions.Get<LevelDefinition>(levelModel.LevelId);
        if (levelDef is null)
        {
            return;
        }
        
        _waves.AddRange(levelDef.Waves);
        _waves.RemoveAll(x => levelModel.SpawnedWaveIds.Contains(x.Id));
        SpawnWave();
    }

    private void SpawnWave()
    {
        if (_waves.IsNullOrEmpty())
        {
            return;
        }

        var wave = _waves[0];
        _waves.RemoveAt(0);
        Log.Debug($"Spawning wave '{wave.Id}'.", nameof(Spawner));

        var viewportSize = GetViewportRect().Size;
        var parent = Session.Get<SessionReferences>().EnemyParent;

        var screenCenter = viewportSize / 2f;
        foreach (var waveEntry in wave.Entries)
        {
            if (waveEntry.EntityDefinition is null)
            {
                continue;
            }

            float radius = Mathf.Max(viewportSize.X, viewportSize.Y);
            float seed = _rng.RandfRange(0f, Mathf.Pi * 2f);
            var startingPosition = screenCenter + new Vector2(Mathf.Cos(seed), Mathf.Sin(seed)) * radius;
            startingPosition = screenCenter + new Vector2(Mathf.Cos(seed), Mathf.Sin(seed)) * 300f; // TODO: delete this row
            var startDir = (startingPosition - screenCenter).Normalized();
            var rowStartOffsetDir = new Vector2(startDir.Y, -startDir.X);

            Enemy leader = null; // TODO: figure out how to get these, maybe something like Dictiontry<Type, object> in entities and they will register themselves there?
            HashSet<Enemy> members = new();

            int enemiesPerRow = 1;
            int rowCount = 0;
            var pos = startingPosition;
            
            for (int i = 0; i < waveEntry.Count; i++)
            {
                var entity = EntitySystem.Spawn(waveEntry.EntityDefinition, parent, pos);
                rowCount++;

                pos += -rowStartOffsetDir * 50f;

                if (rowCount % enemiesPerRow == 0)
                {
                    rowCount = 0;
                    enemiesPerRow++;

                    startingPosition += startDir * 50f;
                    pos = startingPosition;
                    pos += rowStartOffsetDir * ((enemiesPerRow - 1) * 25f);
                }
            }
        }
        
        Session.Get<LevelModel>().SpawnedWaveIds.Add(wave.Id);
    }

    private Vector2 GetSpawnDir()
    {
        return new Vector2(
            _rng.Randf() >= 0f ? 1f : -1f,
            _rng.Randf() >= 0f ? 1f : -1f
        );
    }
}