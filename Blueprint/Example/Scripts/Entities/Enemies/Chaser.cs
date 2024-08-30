using Godot;
using Samurai.Application;
using Samurai.Example.Entities.Player;

namespace Samurai.Example.Entities.Enemies;

public partial class Chaser : Area2D
{
	[Export]
	private float _speed = 50f;
	[Export]
	private Node2D _root;
	
	private PlayerModel _model;

	#region Lifecycle

	public override void _Ready()
	{
		base._Ready();

		_model = Session.Get<PlayerModel>();
		AreaEntered += OnAreaEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		// TODO: this needs to be a CharacterBody2D or something that can collide
		base._PhysicsProcess(delta);

		if (_model.Player is null)
		{
			return;
		}
		
		var playerPosition = _model.Player.GlobalPosition;
		var direction = (playerPosition - _root.GlobalPosition).Normalized();
		_root.GlobalPosition += direction * _speed * (float)delta;
	}

	#endregion Lifecycle

	#region Events

	private void OnAreaEntered(Area2D area)
	{
		
	}

	#endregion Events
}
