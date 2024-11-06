using Godot;
using Samurai.Application;

namespace Samurai.Example.Weapons.Defs;

[GlobalClass]
public partial class ProjectileDefinition : Definition
{
    [Export]
    public PackedScene Prefab;
    [Export]
    public int Damage = 1;
    [Export]
    public bool RemoveOnImpact = true;
}