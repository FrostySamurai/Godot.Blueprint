using Godot;
using Samurai.Application;
using Samurai.Example.Enemies.Defs;

namespace Samurai.Example.UI;

public partial class DebugUi : CanvasLayer
{
    [Export]
    private Slider _cohesionSlider;
    [Export]
    private Slider _separationSlider;
    [Export]
    private Slider _alignmentSlider;

    private FlockingConfig _config;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        _config = Definitions.Config<FlockingConfig>();

        _cohesionSlider.Value = _config.CohesionMultiplier;
        _separationSlider.Value = _config.SeparationMultiplier;
        _alignmentSlider.Value = _config.AlignmentMultiplier;
        
        _cohesionSlider.ValueChanged += OnCohesionChanged;
        _separationSlider.ValueChanged += OnSeparationChanged;
        _alignmentSlider.ValueChanged += OnAlignmentChanged;
    }

    private void OnCohesionChanged(double value)
    {
        _config.CohesionMultiplier = (float)value;
    }

    private void OnSeparationChanged(double value)
    {
        _config.SeparationMultiplier = (float)value;
    }

    private void OnAlignmentChanged(double value)
    {
        _config.AlignmentMultiplier = (float)value;
    }
}