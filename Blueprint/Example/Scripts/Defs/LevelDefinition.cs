using Godot;
using Samurai.Application;
using Samurai.Example.Entities.Enemies.Defs;

namespace Samurai.Example.Defs;

[GlobalClass]
public partial class LevelDefinition : Definition
{
    [Export]
    public WaveDefinition[] Waves;
}