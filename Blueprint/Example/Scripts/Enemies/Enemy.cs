using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Enemies.Data;
using Samurai.Example.Enemies.Defs;
using Samurai.Example.Entities;
using Samurai.Example.Player;

namespace Samurai.Example.Enemies;

public partial class Enemy : CharacterBody2D, IEntityComponent
{
	[Export]
	private float _minSpeed = 50f;
	[Export]
	private float _speed = 50f;
	[Export]
	private Entity _entity;
	[Export]
	private Area2D _flockDetector;
	[Export]
	private CollisionShape2D _flockDetectorShape;

	private FlockingData _flocking;
	private FlockingConfig _config;
	private Node2D _target;

	public string EntityId => _entity.Id;
	
	#region Lifecycle

	public override void _EnterTree()
	{
		_config = Definitions.Config<FlockingConfig>();
		
		var generator = new RandomNumberGenerator();
		var heading = new Vector2(generator.RandfRange(-1f, 1f), generator.RandfRange(-1f, 1f)).Normalized();
		Velocity = heading * _speed;

		Session.Get<EntityModel>().TryGetComponent(_entity.Id, out _flocking);
		_flocking.Self = this;

		float radius = Mathf.Max(_config.AlignmentRadius, Mathf.Max(_config.SeparationRadius, _config.CohesionRadius));
		((CircleShape2D)_flockDetectorShape.Shape).Radius = radius;

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
			if (body is PlayerController)
			{
				_target = body;
			}
            
			return;
		}

		_flocking.Flock.Add(enemy);
	}

	private void BodyExited(Node2D body)
	{
		if (body is not Enemy enemy)
		{
			if (body is PlayerController)
			{
				_target = null;
			}
			
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
		
		DrawCircle(Vector2.Zero, _config.AlignmentRadius, new Color(0f, 0f, 0.5f, 0.3f));
		DrawCircle(Vector2.Zero, _config.SeparationRadius, new Color(0.5f, 0f, 0f, 0.3f));
		DrawCircle(Vector2.Zero, _config.CohesionRadius, new Color(0f, 0.5f, 0f, 0.3f));
		
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
		// TODO: debug purposes
		float radius = Mathf.Max(_config.AlignmentRadius, Mathf.Max(_config.SeparationRadius, _config.CohesionRadius));
		((CircleShape2D)_flockDetectorShape.Shape).Radius = radius;
		
		var accel = Vector2.Zero;
		if (_flocking.Flock.Count > 0)
		{
			// cohesion
			var centerDir = (_flocking.Center - GlobalPosition);
			accel += (centerDir.Normalized() * _speed - Velocity).LimitLength(_config.MaxSteerForce) * _config.CohesionMultiplier;

			// separation
			accel += (_flocking.Separation.Normalized() * _speed - Velocity).LimitLength(_config.MaxSteerForce) * _config.SeparationMultiplier;

			// alignment
			accel += (_flocking.Alignment.Normalized() * _speed - Velocity).LimitLength(_config.MaxSteerForce) * _config.AlignmentMultiplier;
		}

		if (_target is not null)
		{
			var targetDir = (_target.GlobalPosition - GlobalPosition).Normalized();
			float dot = targetDir.Dot(-GlobalTransform.X);
			
			// accel += (targetDir * _speed - Velocity).LimitLength(_config.MaxSteerForce) * 5f;
		}

		if (accel == Vector2.Zero)
		{
			accel = Velocity;
		}
		
		Velocity += accel * (float)delta;
		float speed = Velocity.Length();
		var dir = Velocity / speed;
		speed = Mathf.Clamp(speed, _minSpeed, _speed);
		Velocity = dir * speed;
		
		// Velocity *= _speed * (float)delta; // TODO: resolve issue with them slowing down
		// Velocity = Velocity.LimitLength(_speed);
		
		// MoveAndSlide();

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
