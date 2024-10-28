using Godot;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class WaveEntry : Resource
{
    [Export]
    public EntityDefinition EntityDefinition;
    [Export]
    public int Count;
}