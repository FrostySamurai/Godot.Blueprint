using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;

namespace Samurai.Example.Player;

public partial class PlayerController : CharacterBody2D
{
	public const float Speed = 300.0f;
	
	[Export]
	private Node2D _weaponParent;

	private PlayerModel _model;
	private Weapon _currentWeapon;

	#region Lifecycle

	public override void _EnterTree()
	{
		base._EnterTree();

		_model = Session.Get<PlayerModel>();
		SpawnWeapon();
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("fire"))
		{
			_currentWeapon?.Fire();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var directionToMouse = (GetGlobalMousePosition() - GlobalPosition).Normalized();
		Rotation = Vector2.Up.AngleTo(directionToMouse);
		
		Vector2 velocity = Velocity;
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
		if (direction != Vector2.Zero)
		{
			velocity = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	#endregion Lifecycle

	#region Private

	private void SpawnWeapon()
	{
		if (_currentWeapon is not null)
		{
			NodePool.Return(_currentWeapon);
		}

		var def = _model.Weapon;
		if (def is null)
		{
			return;
		}

		_currentWeapon = NodePool.Retrieve<Weapon>(def.Prefab, _weaponParent);
		_currentWeapon.Init(def);
	}

	#endregion Private
}
