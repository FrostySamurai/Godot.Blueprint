using Godot;
using Samurai.Application;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class HealthDefinition : Definition
{
    [Export]
    public int MaxHealth;
}