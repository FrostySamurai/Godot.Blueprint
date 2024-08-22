using Godot;
using Samurai.Application;

namespace Samurai.Example.Player.Defs;

[GlobalClass]
public partial class WeaponDefinition : Definition
{
    [Export]
    public double FireRate;
    [Export]
    public PackedScene Prefab;
    [Export]
    public ProjectileDefinition[] Projectiles;
}