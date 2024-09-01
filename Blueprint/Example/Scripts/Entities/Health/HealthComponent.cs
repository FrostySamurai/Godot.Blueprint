using Godot;
using Samurai.Application;
using Samurai.Example.Entities.Health.Data;
using Samurai.Example.Entities.Health.Defs;

namespace Samurai.Example.Entities.Health;

public partial class HealthComponent : Area2D
{
    private const string LogTag = nameof(Health);

    [Export]
    private Entity _entity;
    [Export]
    private HealthDefinition _definition;

    private EntityModel _entityModel;

    public override void _EnterTree()
    {
        base._EnterTree();

        if (_entity is null)
        {
            return;
        }

        _entityModel ??= Session.Get<EntityModel>();
        if (!_entityModel.TryGetComponent<HealthData>(_entity.Id, out var data))
        {
            data = new HealthData
            {
                DefinitionId = _definition.Id,
                Current = _definition.MaxHealth
            };
            
            _entityModel.AddComponent(_entity.Id, data);
        }
    }

    public void TakeDamage(int amount)
    {
        if (!_entityModel.TryGetComponent<HealthData>(_entity.Id, out var data))
        {
            return;
        }

        int old = data.Current;
        data.Current = Mathf.Max(0, data.Current - amount);
        Log.Debug($"'{Name}' took {amount} of damage. Remaining: {data.Current}.", LogTag);
        Session.Events.Raise(new HealthEvents.OnDamageTaken(old, data.Current, amount));
        if (data.Current <= 0)
        {
            Log.Debug($"'{_entity.Name}' - '{_entity.Id}' died.", LogTag);
            Session.Events.Raise(new HealthEvents.OnDestroyed(_entity.Id));
        }
    }
}