using Godot;
using Samurai.Example.Entities.Health.Data;

namespace Samurai.Example.Entities.Health.Defs;

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