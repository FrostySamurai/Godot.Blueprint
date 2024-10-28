using Godot;

namespace Samurai.Example.Entities.Enemies.Defs;

[GlobalClass]
public partial class WaveEntry : Resource
{
    [Export]
    public EntityDefinition EntityDefinition;
    [Export]
    public int Count;
}