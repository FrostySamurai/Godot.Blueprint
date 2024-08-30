using Godot;

namespace Samurai.Example.Entities.Enemies.Defs;

[GlobalClass]
public partial class WaveEntry : Resource
{
    [Export]
    public PackedScene Prefab;
    [Export]
    public int Count;
}