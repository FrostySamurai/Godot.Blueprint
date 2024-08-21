using Godot;
using Samurai.Application;
using Samurai.Example.Enemies.Defs;

namespace Samurai.Example.Enemies;

public partial class Health : Area2D
{
    private const string LogTag = nameof(Health);
    
    [Export]
    private string _definitionId;

    private int _health;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        if (!Definitions.TryGet<HealthDefinition>(_definitionId, out var definition))
        {
            Log.Error($"No definition exists for definition id '{_definitionId}'. Name: {Name}", LogTag);
            return;
        }

        _health = definition.MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        _health = Mathf.Max(0, _health - amount);
        Log.Debug($"'{Name}' took {amount} of damage. Remaining: {_health}.", LogTag);
        if (_health <= 0)
        {
            Log.Debug($"'{Name}' died.", LogTag);
            QueueFree();
        }
    }
}