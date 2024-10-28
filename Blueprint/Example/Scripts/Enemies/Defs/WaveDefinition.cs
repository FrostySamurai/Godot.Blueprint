using Godot;
using Samurai.Application;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class WaveDefinition : Definition
{
    [Export]
    public WaveEntry[] Entries;
}