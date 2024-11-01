using System.Runtime.CompilerServices;
using Godot;
using Samurai.Application;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies;

public static class FlockingSystem
{
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
        foreach (var entry in flocking.Flock)
        {
            alignmentDir += entry.Velocity;
            centerPos += entry.GlobalPosition;

            if (flocking.Self is null)
            {
                continue;
            }

            float distance = flocking.Self.GlobalPosition.DistanceTo(entry.GlobalPosition);
            var reverseDir = (flocking.Self.GlobalPosition - entry.GlobalPosition) / distance;
            separation += reverseDir;
        }

        int flockSize = flocking.Flock.Count;
        if (flockSize > 0)
        {
            alignmentDir /= flockSize;
            centerPos /= flockSize;
            separation /= flockSize;
        }

        flocking.Alignment = alignmentDir;
        flocking.Center = centerPos;
        flocking.Separation = separation;
    }
}