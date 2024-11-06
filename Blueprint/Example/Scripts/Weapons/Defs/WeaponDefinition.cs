using Godot;
using Samurai.Application;

namespace Samurai.Example.Weapons.Defs;

[GlobalClass]
public partial class WeaponDefinition : Definition
{
    [Export]
    public PackedScene Prefab;
}