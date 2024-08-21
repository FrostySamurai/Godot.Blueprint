using Godot;
using Samurai.Application.Pooling;
using Samurai.Example.Enemies;

namespace Samurai.Example.Player;

public partial class Projectile : Area2D
{
	[Export]
	private float _speed = 100f;

	public override void _EnterTree()
	{
		base._EnterTree();

		AreaEntered += OnAreaEntered;
	}

	public override void _Process(double delta)
	{
		base._PhysicsProcess(delta);

		Position += -Transform.Y * _speed * (float)delta;
	}

	private void OnAreaEntered(Area2D other)
	{
		if (other is not Health destructable)
		{
			return;
		}
		
		destructable.TakeDamage(1);
		NodePool.Return(this);
	}
}