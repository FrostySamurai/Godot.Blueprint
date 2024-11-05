using Godot;
using Samurai.Application;
using Samurai.Example.Enemies.Defs;

namespace Samurai.Example.UI;

public partial class DebugUi : CanvasLayer
{
    [ExportCategory("Labels")]
    [Export]
    private Label _cohesionLabel;
    [Export]
    private Label _separationLabel;
    [Export]
    private Label _alignmentValue;
    [Export]
    private Label _cohesionRangeLabel;
    [Export]
    private Label _separationRangeLabel;
    [Export]
    private Label _alignmentRangeValue;
    
    [ExportCategory("Sliders")]
    [Export]
    private Slider _cohesionSlider;
    [Export]
    private Slider _separationSlider;
    [Export]
    private Slider _alignmentSlider;
    [Export]
    private Slider _cohesionRangeSlider;
    [Export]
    private Slider _separationRangeSlider;
    [Export]
    private Slider _alignmentRangeSlider;

    [ExportCategory("Interaction")]
    [Export]
    private Button _saveButton;

    private FlockingConfig _config;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        _config = Definitions.Config<FlockingConfig>();

        OnCohesionChanged(_config.CohesionMultiplier);
        OnSeparationChanged(_config.SeparationMultiplier);
        OnAlignmentChanged(_config.AlignmentMultiplier);
        OnCohesionRangeChanged(_config.CohesionRadius);
        OnSeparationRangeChanged(_config.SeparationRadius);
        OnAlignmentRangeChanged(_config.AlignmentRadius);

        _cohesionSlider.Value = _config.CohesionMultiplier;
        _separationSlider.Value = _config.SeparationMultiplier;
        _alignmentSlider.Value = _config.AlignmentMultiplier;
        _cohesionRangeSlider.Value = _config.CohesionRadius;
        _separationRangeSlider.Value = _config.SeparationRadius;
        _alignmentRangeSlider.Value = _config.AlignmentRadius;
        
        _cohesionSlider.ValueChanged += OnCohesionChanged;
        _separationSlider.ValueChanged += OnSeparationChanged;
        _alignmentSlider.ValueChanged += OnAlignmentChanged;
        _cohesionRangeSlider.ValueChanged += OnCohesionRangeChanged;
        _separationRangeSlider.ValueChanged += OnSeparationRangeChanged;
        _alignmentRangeSlider.ValueChanged += OnAlignmentRangeChanged;

        _saveButton.Pressed += OnSavePressed;
    }

    private void OnCohesionChanged(double value)
    {
        _config.CohesionMultiplier = (float)value;
        _cohesionLabel.Text = $"{value:F1}";
    }

    private void OnSeparationChanged(double value)
    {
        _config.SeparationMultiplier = (float)value;
        _separationLabel.Text = $"{value:F1}";
    }

    private void OnAlignmentChanged(double value)
    {
        _config.AlignmentMultiplier = (float)value;
        _alignmentValue.Text = $"{value:F1}";
    }

    private void OnCohesionRangeChanged(double value)
    {
        _config.CohesionRadius = (float)value;
        _cohesionRangeLabel.Text = $"{value:F0}";
    }

    private void OnSeparationRangeChanged(double value)
    {
        _config.SeparationRadius = (float)value;
        _separationRangeLabel.Text = $"{value:F0}";
    }

    private void OnAlignmentRangeChanged(double value)
    {
        _config.AlignmentRadius = (float)value;
        _alignmentRangeValue.Text = $"{value:F0}";
    }

    private void OnSavePressed()
    {
        ResourceSaver.Save(_config);
    }
}