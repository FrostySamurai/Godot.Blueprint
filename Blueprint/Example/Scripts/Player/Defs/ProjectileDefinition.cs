using Godot;
using Samurai.Application;

namespace Samurai.Example.Player.Defs;

[GlobalClass]
public partial class ProjectileDefinition : Definition
{
    [Export]
    public int Damage = 1;
    [Export]
    public PackedScene Prefab;
}