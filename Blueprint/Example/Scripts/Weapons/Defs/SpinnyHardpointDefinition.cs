using Godot;
using Samurai.Application;

namespace Samurai.Example.Weapons.Defs;

[GlobalClass]
public partial class SpinnyHardpointDefinition : Definition
{
    [Export]
    public int ProjectileCount = 3;
    [Export]
    public ProjectileDefinition Projectile;
}