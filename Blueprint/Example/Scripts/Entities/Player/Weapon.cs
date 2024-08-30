using Godot;
using RedHerring.Extensions.Collections;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Player.Defs;

namespace Samurai.Example.Entities.Player;

public partial class Weapon : Node2D
{
    internal const string LogTag = "Weapons";
    
    [Export]
    private Node2D[] _spawnPoints;

    private WeaponDefinition _definition;
    private double _lastFiredAt;

    #region Lifecycle

    public void Init(WeaponDefinition def)
    {
        _definition = def;
        if (def is null)
        {
            Log.Error($"No definition passed to weapon '{Name}'.", LogTag);
            return;
        }

        if (def.Projectiles.Length != _spawnPoints.Length)
        {
            Log.Error($"Projectile definitions and spawn points mismatch! Def: '{def.Id}' - Weapon: '{Name}'", LogTag);
        }
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        
        _lastFiredAt = 0d;
        _definition = null;
    }

    #endregion Lifecycle

    public void Fire()
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
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            if (_definition.Projectiles.IsOutOfBounds(i))
            {
                break;
            }

            var projectileDef = _definition.Projectiles[i];
            var projectile = NodePool.Retrieve<Projectile>(projectileDef.Prefab, parent);
            
            projectile.Init(projectileDef);
            projectile.GlobalPosition = _spawnPoints[i].GlobalPosition;
            projectile.SetRotation(_spawnPoints[i].GlobalRotation);
        }
    }
}