using Godot;
using Samurai.Application.Pooling;
using Samurai.Example.Player.Defs;

namespace Samurai.Example.Entities.Player;

public partial class Projectile : Area2D
{
	[Export]
	private float _speed = 100f;

	private ProjectileDefinition _definition;

	#region Lifecycle

	public override void _Ready()
	{
		base._EnterTree();
		
		AreaEntered += OnAreaEntered;
	}

	public void Init(ProjectileDefinition def)
	{
		_definition = def;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
			
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
		if (other is not Health health)
		{
			return;
		}
		
		health.TakeDamage(_definition.Damage);
		CallDeferred(nameof(Return));
	}

	private void Return()
	{
		NodePool.Return(this);
	}
}