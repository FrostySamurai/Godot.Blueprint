using Godot;

namespace Samurai.Example.Weapons.Projectiles;

public partial class SpinnyMover : Node2D
{
    [Export]
    private Projectile _root;
    [Export]
    private float _angularSpeed;
    [Export]
    private float _baseDistance = 75f;
    [Export]
    private float _distanceDelta = 25f;
    [Export]
    private float _pulsationDuration = 1f;

    private float _angularSpeedRadians;
    private float _rotation;
    private float _pulse;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        _angularSpeedRadians = Mathf.DegToRad(_angularSpeed);
        _rotation = _root.Rotation;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        const float RadiansInCircle = 2f * Mathf.Pi;

        var dir = new Vector2(Mathf.Cos(_rotation), Mathf.Sin(_rotation));
        float pulseDistance = Mathf.Sin(_pulse) * _distanceDelta;
        _root.GlobalPosition = _root.Parent.GlobalPosition + dir * (_baseDistance + pulseDistance);

        _rotation += _angularSpeedRadians * (float)delta;
        _pulse += _pulsationDuration * RadiansInCircle * (float)delta;
    }
}