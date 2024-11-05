﻿using Godot;
using Samurai.Application;
using Samurai.Application.Pooling;
using Samurai.Example.Entities;
using Samurai.Example.Health;
using Samurai.Example.Player.Defs;

namespace Samurai.Example.Player;

public partial class Projectile : Area2D
{
	[Export]
	private float _speed = 100f;

	private ProjectileDefinition _definition;

	#region Lifecycle

	public void Init(ProjectileDefinition def)
	{
		_definition = def;
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		
		BodyEntered += OnBodyEntered;
	}

	public override void _ExitTree()
	{
		base._ExitTree();

		BodyEntered -= OnBodyEntered;
		_definition = null;
	}

	#endregion Lifecycle

	public override void _Process(double delta)
	{
		base._PhysicsProcess(delta);

		Position += -Transform.Y * _speed * (float)delta;
	}

	private void OnBodyEntered(Node2D other)
	{
		Log.Debug(other.Name);
		if (other is not IEntityComponent entity)
		{
			return;
		}
		
		HealthSystem.DealDamageTo(entity.EntityId, _definition.Damage);
		CallDeferred(nameof(Return));
	}

	private void Return()
	{
		NodePool.Return(this);
	}
}