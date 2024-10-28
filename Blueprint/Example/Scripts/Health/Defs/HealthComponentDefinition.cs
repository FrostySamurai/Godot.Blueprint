using Godot;
using Samurai.Example.Entities;
using Samurai.Example.Health.Data;

namespace Samurai.Example.Health.Defs;

[GlobalClass]
public partial class HealthComponentDefinition : EntityComponentDefinition
{
    [Export]
    public int MaxHealth;

    public override IComponentData Create()
    {
        return new HealthData(this);
    }
}