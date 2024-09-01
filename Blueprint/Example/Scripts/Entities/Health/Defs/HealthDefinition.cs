using Godot;
using Samurai.Application;

namespace Samurai.Example.Entities.Health.Defs;

[GlobalClass]
public partial class HealthDefinition : Definition
{
    [Export]
    public int MaxHealth;
}