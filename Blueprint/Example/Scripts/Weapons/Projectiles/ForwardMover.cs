using Godot;

namespace Samurai.Example.Weapons.Projectiles;

public partial class ForwardMover : Node2D
{
    [Export]
    private Node2D _root;
    [Export]
    private float _speed = 100f;
	
    public override void _Process(double delta)
    {
        base._PhysicsProcess(delta);

        _root.Position += -_root.Transform.Y * _speed * (float)delta;
    }
}