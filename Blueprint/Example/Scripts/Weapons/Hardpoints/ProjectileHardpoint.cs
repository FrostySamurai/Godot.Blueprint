using System.Runtime.CompilerServices;
using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Weapons.Defs;
using Samurai.Example.Weapons.Projectiles;

namespace Samurai.Example.Weapons.Hardpoints;

public partial class ProjectileHardpoint : Node2D
{
    [Export]
    private ProjectileHardpointDefinition _definition;
    [Export]
    private Weapon _weapon;
    [Export]
    private Node2D[] _spawnPoints;

    private double _lastFiredAt = 0d;

    #region Lifecycle

    public override void _ExitTree()
    {
        _lastFiredAt = 0d;
    }

    public override void _Process(double delta)
    {
        if (_weapon.IsActive)
        {
            Fire();
        }
    }

    #endregion Lifecycle

    #region Private

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Fire()
    {
        if (_definition is null)
        {
            return;
        }

        double time = Time.GetTicksMsec() / 1000d;
        if (_lastFiredAt > 0d && time - _lastFiredAt < _definition.FireCooldown)
        {
            return;
        }

        _lastFiredAt = time;
        var parent = Session.Get<SessionReferences>().ProjectileParent;
        foreach (var entry in _spawnPoints)
        {
            var projectile = NodePool.Retrieve<Projectile>(_definition.Projectile.Prefab, parent);
            projectile.Init(_definition.Projectile);
            projectile.GlobalPosition = entry.GlobalPosition;
            projectile.SetRotation(entry.GlobalRotation);
        }

        #endregion Private
    }
}