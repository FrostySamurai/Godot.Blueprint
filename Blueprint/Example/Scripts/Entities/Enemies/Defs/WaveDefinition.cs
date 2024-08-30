using Godot;
using Samurai.Application;

namespace Samurai.Example.Entities.Enemies.Defs;

[GlobalClass]
public partial class WaveDefinition : Definition
{
    [Export]
    public WaveEntry[] Entries;
}