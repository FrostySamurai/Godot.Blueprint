using Godot;
using Samurai.Application;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class FlockingConfig : Config
{
    [Export]
    public float CohesionMultiplier = 1f;
    [Export]
    public float SeparationMultiplier = 1f;
    [Export]
    public float AlignmentMultiplier = 1f;
}