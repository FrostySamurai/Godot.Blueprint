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

	private Vector2 _heading;
	private FlockingData _flocking;
	
	#region Lifecycle

	public override void _EnterTree()
	{
		var generator = new RandomNumberGenerator();
		_heading = new Vector2(generator.RandfRange(-1f, 1f), generator.RandfRange(-1f, 1f)).Normalized();

		Session.Get<EntityModel>().TryGetComponent(_entity.Id, out _flocking);

		((CircleShape2D)_flockDetectorShape.Shape).Radius = _flocking.Definition.Radius;

		_flockDetector.BodyEntered += BodyEntered;
		_flockDetector.BodyExited += BodyExited;
		Velocity = _heading * _speed;
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		
		_flockDetector.BodyEntered -= BodyEntered;
		_flockDetector.BodyExited -= BodyExited;
	}

	private void BodyEntered(Node2D body)
	{
		if (body is not Enemy enemy)
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
			DrawCircle(_flocking.FlockCenter - GlobalPosition, 5f, new Color(0f, 1f, 0f, 0.7f));
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		// if (_flocking.IsDebug)
		// {
			var accel = Vector2.Zero;
		
			var center = Vector2.Zero;
			foreach (var entry in _flocking.Flock)
			{
				center += entry.GlobalPosition;
			}

			if (_flocking.Flock.Count > 0)
			{
				center /= _flocking.Flock.Count;
			}

			_flocking.FlockCenter = center;
			
			var dir = (center - GlobalPosition).Normalized();
			accel += (dir * _speed - Velocity).LimitLength(8f) * 5f;
			
			Velocity += accel * (float)delta;
			float speed = Velocity.Length();
			dir = Velocity / speed;
			speed = Mathf.Clamp (speed, _speed / 2f, _speed);
			Velocity = dir * speed;
		// }
		
		MoveAndSlide();

		var position = GlobalPosition;
		var viewportSize = GetViewportRect().Size;
		if (position.X < 0f) position.X = viewportSize.X;
		else if (position.X > viewportSize.X) position.X = 0f;

		if (position.Y < 0f) position.Y = viewportSize.Y;
		else if (position.Y > viewportSize.Y) position.Y = 0f;

		GlobalPosition = position;

		QueueRedraw(); // debug purposes

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
