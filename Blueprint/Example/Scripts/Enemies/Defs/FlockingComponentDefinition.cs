using Godot;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class FlockingComponentDefinition : EntityComponentDefinition
{
    [Export]
    public float Radius = 30f;
    
    public override IComponentData Create()
    {
        return new FlockingData(this);
    }
}