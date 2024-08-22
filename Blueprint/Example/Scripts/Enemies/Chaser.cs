using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Player;

namespace Samurai.Example.Enemies;

public partial class Chaser : Area2D
{
	[Export]
	private float _speed = 50f;
	[Export]
	private Health _health;
	
	private PlayerModel _model;
	
	public override void _Ready()
	{
		base._Ready();

		_model = Session.Get<PlayerModel>();
		_health.OnDestroyed += Return;
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);

		if (_model.Player is null)
		{
			return;
		}
		
		var playerPosition = _model.Player.GlobalPosition;
		var direction = (playerPosition - GlobalPosition).Normalized();
		GlobalPosition += direction * _speed * (float)delta;
	}

	private void Return()
	{
		NodePool.Return(this);
	}
}
