﻿using Godot;

namespace Samurai.Example.Enemies;

public partial class EnemyManager : Node2D 
{
    public override void _EnterTree()
    {
        base._EnterTree();
        
        FlockingSystem.Init();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        
        FlockingSystem.Tick();
    }
}