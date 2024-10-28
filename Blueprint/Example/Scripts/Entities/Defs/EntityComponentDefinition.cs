using Samurai.Application;

namespace Samurai.Example.Entities;

public abstract partial class EntityComponentDefinition : Definition
{
    public abstract IComponentData Create();
}