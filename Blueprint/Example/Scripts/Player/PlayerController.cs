using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;

namespace Samurai.Example.Player;

public partial class PlayerController : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	[Export]
	private PackedScene _projectilePrefab;

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("fire"))
		{
			var parent = Session.Get<SessionReferences>().ProjectileParent;
			var projectile = NodePool.Retrieve<Projectile>(_projectilePrefab, parent);
			projectile.GlobalPosition = GlobalPosition;
			projectile.SetRotation(Rotation);
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
}
