using Godot;
using Samurai.Application.Pooling;
using Samurai.Example.Enemies;
using Samurai.Example.Player.Defs;

namespace Samurai.Example.Player;

public partial class Projectile : Area2D, IPoolReturnable
{
	[Export]
	private float _speed = 100f;

	private ProjectileDefinition _definition;

	#region Lifecycle

	public void Init(ProjectileDefinition def)
	{
		_definition = def;
		AreaEntered += OnAreaEntered;
	}

	public void OnReturnToPool()
	{
		AreaEntered -= OnAreaEntered;
		_definition = null;
	}

	#endregion Lifecycle

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
		
		destructable.TakeDamage(_definition.Damage);
		CallDeferred(nameof(Return));
	}

	private void Return()
	{
		NodePool.Return(this);
	}
}