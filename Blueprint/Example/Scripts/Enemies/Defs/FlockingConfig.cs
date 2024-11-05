using Godot;
using Samurai.Application;

namespace Samurai.Example.Enemies.Defs;

[GlobalClass]
public partial class FlockingConfig : Config
{
    [ExportCategory("Radii")]
    [Export]
    public float CohesionRadius = 20f;
    [Export]
    public float SeparationRadius = 10f;
    [Export]
    public float AlignmentRadius = 30f;
    
    [ExportCategory("Multipliers")]
    [Export]
    public float CohesionMultiplier = 1f;
    [Export]
    public float SeparationMultiplier = 1f;
    [Export]
    public float AlignmentMultiplier = 1f;
}