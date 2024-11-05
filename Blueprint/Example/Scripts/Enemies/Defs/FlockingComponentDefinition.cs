using Godot;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class FlockingComponentDefinition : EntityComponentDefinition
{
    // TODO: add radius overrides or maybe into specific enemy's component? (like flock leader)
    
    public override IComponentData Create()
    {
        return new FlockingData(this);
    }
}