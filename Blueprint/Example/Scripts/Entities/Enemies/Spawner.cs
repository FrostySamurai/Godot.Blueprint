using System.Collections.Generic;
using Godot;
using RedHerring.Extensions.Collections;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Defs;
using Samurai.Example.Entities.Enemies.Defs;

namespace Samurai.Example.Entities.Enemies;

public partial class Spawner : Node2D
{
    private readonly List<WaveDefinition> _waves = new();
    
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
        var rng = new RandomNumberGenerator();
        foreach (var waveEntry in wave.Entries)
        {
            if (waveEntry.Prefab is null)
            {
                continue;
            }
            
            for (int i = 0; i < waveEntry.Count; i++)
            {
                var instance = NodePool.Retrieve<Enemy>(waveEntry.Prefab, parent);

                int xSide = Mathf.Sign(rng.Randf() - 0.5f);
                xSide = xSide == 0 ? 1 : xSide;
                int ySide = Mathf.Sign(rng.Randf() - 0.5f);
                ySide = ySide == 0 ? 1 : ySide;

                bool longSideX = rng.Randf() >= 0.5f;

                float xPos = Mathf.Max(0, xSide) * viewportSize.X + xSide * rng.RandfRange(0f, 100f);
                float yPos = Mathf.Max(0, ySide) * viewportSize.Y + ySide * rng.RandfRange(0f, 100f);

                if (longSideX)
                {
                    xPos = rng.RandfRange(-100f, viewportSize.X + 100f);
                }
                else
                {
                    yPos = rng.RandfRange(-100f, viewportSize.Y + 100f);
                }

                instance.GlobalPosition = new Vector2(xPos, yPos);
            }
        }
    }
}