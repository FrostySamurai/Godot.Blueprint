using Godot;
using Samurai.Application;

namespace Samurai.Example.Entities.Defs;

[GlobalClass]
public partial class HealthDefinition : Definition
{
    [Export]
    public int MaxHealth;
}