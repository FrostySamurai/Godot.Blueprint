using System.Runtime.CompilerServices;
using Godot;
using Samurai.Application;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Enemies.Defs;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies;

public static class FlockingSystem
{
    private static FlockingConfig _config;

    public static void Init()
    {
        _config = Definitions.Config<FlockingConfig>();
    }
    
    public static void Tick()
    {
        var model = Session.Get<EntityModel>();
        foreach (var entry in model.GetAllComponents<FlockingData>())
        {
            Update(entry);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Update(FlockingData flocking)
    {
        var alignmentDir = Vector2.Zero;
        var centerPos = Vector2.Zero;
        var separation = Vector2.Zero;

        int alignmentCount = 0;
        int cohesionCount = 0;
        int separationCount = 0;
        foreach (var entry in flocking.Flock)
        {
            if (flocking.Self is null)
            {
                continue;
            }

            float distance = flocking.Self.GlobalPosition.DistanceTo(entry.GlobalPosition);
            if (distance <= _config.AlignmentRadius)
            {
                alignmentDir += entry.Velocity;
                alignmentCount++;
            }

            if (distance <= _config.CohesionRadius)
            {
                centerPos += entry.GlobalPosition;
                cohesionCount++;
            }

            if (distance <= _config.SeparationRadius)
            {
                var reverseDir = (flocking.Self.GlobalPosition - entry.GlobalPosition) / distance;
                separation += reverseDir;
                separationCount++;
            }
        }

        if (alignmentCount > 0) alignmentDir /= alignmentCount;
        if (cohesionCount > 0) centerPos /= cohesionCount;
        if (separationCount > 0) separation /= separationCount;

        flocking.Alignment = alignmentDir;
        flocking.Center = centerPos;
        flocking.Separation = separation;
    }
}