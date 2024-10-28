using Godot;
using Samurai.Application;

namespace Samurai.Example.Entities;

[GlobalClass]
public partial class EntityDefinition : Definition
{
    [Export]
    public PackedScene Prefab;
    [Export]
    public EntityComponentDefinition[] Components;
}