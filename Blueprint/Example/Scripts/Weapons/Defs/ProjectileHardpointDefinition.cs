using Godot;
using Samurai.Application;

namespace Samurai.Example.Weapons.Defs;

[GlobalClass]
public partial class ProjectileHardpointDefinition : Definition
{
    [Export]
    public double FireCooldown = 1d;
    [Export]
    public ProjectileDefinition Projectile;
}