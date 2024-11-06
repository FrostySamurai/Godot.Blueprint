﻿using Godot;
using Samurai.Application.Pooling;
using Samurai.Example.Entities;
using Samurai.Example.Health;
using Samurai.Example.Weapons.Defs;

namespace Samurai.Example.Weapons.Projectiles;

public partial class Projectile : Area2D
{
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

	#region Events

	private void OnBodyEntered(Node2D other)
	{
		if (other is not IEntityComponent entity)
		{
			return;
		}
		
		HealthSystem.DealDamageTo(entity.EntityId, _definition.Damage);
		if (_definition.RemoveOnImpact)
		{
			NodePool.Return(this);
		}
	}

	#endregion Events
}