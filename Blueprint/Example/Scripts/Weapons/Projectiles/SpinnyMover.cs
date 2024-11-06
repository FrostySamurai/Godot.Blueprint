using Godot;

namespace Samurai.Example.Weapons.Projectiles;

public partial class SpinnyMover : Node2D
{
    [Export]
    private Projectile _root;
    [Export]
    private float _angularSpeed;

    private float _angularSpeedRadians;
    private float _rotation;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        _angularSpeedRadians = Mathf.DegToRad(_angularSpeed);
        _rotation = _root.Rotation;
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var dir = new Vector2(Mathf.Cos(_rotation), Mathf.Sin(_rotation));
        _root.GlobalPosition = _root.Parent.GlobalPosition + dir * 100f;

        _rotation += _angularSpeedRadians * (float)delta;
    }
}