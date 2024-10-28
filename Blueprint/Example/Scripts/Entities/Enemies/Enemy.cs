using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Entities.Player;

namespace Samurai.Example.Entities.Enemies;

public partial class Enemy : CharacterBody2D
{
	[Export]
	private float _speed = 50f;
	
	private PlayerModel _model;

	private bool _isActive;

	#region Lifecycle

	public override void _EnterTree()
	{
		_model ??= Session.Get<PlayerModel>();
		_isActive = true;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_isActive)
		{
			return;
		}
		
		base._PhysicsProcess(delta);

		if (_model.Player is null)
		{
			return;
		}
		
		var playerPosition = _model.Player.GlobalPosition;
		var direction = (playerPosition - GlobalPosition).Normalized();
		Velocity = direction * _speed;
		MoveAndSlide();

		// var collision = MoveAndCollide(Velocity * (float)delta);
		// if (collision?.GetCollider() is PlayerController player)
		// {
		// 	Log.Debug("Dealing damage to player.");
		// 	CallDeferred(nameof(Return));
		// 	_isActive = false;
		// }
	}

	#endregion Lifecycle

	#region Private

	private void Return()
	{
		NodePool.Return(this);
	}

	#endregion Private
}
