using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Entities;

namespace Samurai.Example.Enemies;

public partial class Enemy : CharacterBody2D
{
	[Export]
	private float _speed = 50f;
	[Export]
	private Entity _entity;
	[Export]
	private Area2D _flockDetector;
	[Export]
	private CollisionShape2D _flockDetectorShape;

	private FlockingData _flocking;
	
	#region Lifecycle

	public override void _EnterTree()
	{
		var generator = new RandomNumberGenerator();
		var heading = new Vector2(generator.RandfRange(-1f, 1f), generator.RandfRange(-1f, 1f)).Normalized();
		Velocity = heading * _speed;

		Session.Get<EntityModel>().TryGetComponent(_entity.Id, out _flocking);
		_flocking.Self = this;

		((CircleShape2D)_flockDetectorShape.Shape).Radius = _flocking.Definition.Radius;

		_flockDetector.BodyEntered += BodyEntered;
		_flockDetector.BodyExited += BodyExited;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		
		_flockDetector.BodyEntered -= BodyEntered;
		_flockDetector.BodyExited -= BodyExited;
	}

	private void BodyEntered(Node2D body)
	{
		if (body == this || body is not Enemy enemy)
		{
			return;
		}

		_flocking.Flock.Add(enemy);
	}

	private void BodyExited(Node2D body)
	{
		if (body is not Enemy enemy)
		{
			return;
		}

		_flocking.Flock.Remove(enemy);
	}

	public override void _Draw()
	{
		base._Draw();

		const float RayLength = 40f;
		DrawLine(Vector2.Zero, Velocity.Normalized() * RayLength, new Color(1f, 0f, 0f, 0.5f));
		
		if (!_flocking.IsDebug)
		{
			return;
		}
		
		DrawCircle(Vector2.Zero, _flocking.Definition.Radius, new Color(0.1f, 0.1f, 0.1f, 0.3f));
		foreach (var entry in _flocking.Flock)
		{
			DrawLine(Vector2.Zero, entry.GlobalPosition - GlobalPosition, new Color(1f, 1f, 1f, 0.5f));
		}

		if (_flocking.Flock.Count > 0)
		{
			var center = _flocking.Center - GlobalPosition;
			DrawCircle(center, 5f, new Color(0f, 1f, 0f, 0.7f));
			DrawLine(center, center + _flocking.Alignment, new Color(0f, 1f, 1f, 0.7f));
			DrawLine(Vector2.Zero, _flocking.Separation, new Color(1f, 0f, 0f, 0.7f));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		var accel = Vector2.Zero;
		if (_flocking.Flock.Count > 0)
		{
			// cohesion
			var centerDir = (_flocking.Center - GlobalPosition);
			accel += (centerDir.Normalized() * _speed - Velocity);

			// separation
			accel += (_flocking.Separation.Normalized() * _speed - Velocity) * 1f;

			// alignment
			accel += (_flocking.Alignment.Normalized() * _speed - Velocity) * 1.5f;
		}

		if (accel == Vector2.Zero)
		{
			accel = Velocity;
		}
		
		Velocity += accel/*.Normalized() * _speed*/ * (float)delta;
		Velocity = Velocity.LimitLength(_speed);
		
		MoveAndSlide();

		var position = GlobalPosition;
		var viewportSize = GetViewportRect().Size;
		if (position.X < 0f) position.X = viewportSize.X;
		else if (position.X > viewportSize.X) position.X = 0f;

		if (position.Y < 0f) position.Y = viewportSize.Y;
		else if (position.Y > viewportSize.Y) position.Y = 0f;

		GlobalPosition = position;

		QueueRedraw(); // debug purposes
	}

	#endregion Lifecycle

	#region Private

	private void Return()
	{
		NodePool.Return(this);
	}

	#endregion Private
}
