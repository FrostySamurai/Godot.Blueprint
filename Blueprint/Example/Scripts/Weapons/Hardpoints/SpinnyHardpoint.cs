using System.Collections.Generic;
using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Application.Utility;
using Samurai.Example.Weapons.Defs;
using Samurai.Example.Weapons.Projectiles;

namespace Samurai.Example.Weapons.Hardpoints;

public partial class SpinnyHardpoint : Node2D
{
    [Export]
    private SpinnyHardpointDefinition _definition;

    private List<Projectile> _projectiles = new();
    
    public override void _EnterTree()
    {
        base._EnterTree();

        var parent = Session.Get<SessionReferences>().ProjectileParent;
        for (int i = 0; i < _definition.ProjectileCount; i++)
        {
            var instance = NodePool.RetrieveParentless<Projectile>(_definition.Projectile.Prefab);
            instance.RotationDegrees = i * (360f / _definition.ProjectileCount);
            instance.Init(_definition.Projectile, this);
            instance.ReparentSafe(parent);
            _projectiles.Add(instance);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        NodePool.Return(_projectiles);
        _projectiles.Clear();
    }
}