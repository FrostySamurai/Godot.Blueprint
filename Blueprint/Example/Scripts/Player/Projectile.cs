using Godot;

namespace Samurai.Example.Player;

public partial class Projectile : Area2D
{
	[Export]
	private float _speed = 100f;

	public override void _Process(double delta)
	{
		base._PhysicsProcess(delta);

		Position += -Transform.Y * _speed * (float)delta;
	}
}