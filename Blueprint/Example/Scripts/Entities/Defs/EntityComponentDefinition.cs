using Godot;
using Samurai.Application;

namespace Samurai.Example.Entities;

public abstract partial class EntityComponentDefinition : Definition
{
    [Export]
    public bool IgnoreInSave = false;
    
    public abstract IComponentData Create();
}