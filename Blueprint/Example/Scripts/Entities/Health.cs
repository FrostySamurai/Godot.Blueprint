using Godot;
using Samurai.Application;
using Samurai.Example.Entities.Defs;

namespace Samurai.Example.Entities;

public partial class Health : Area2D
{
    private const string LogTag = nameof(Health);
    
    [Export]
    private HealthDefinition _definition;

    private int _health;

    public delegate void Destroyed();
    public event Destroyed OnDestroyed;
    
    public override void _EnterTree()
    {
        base._EnterTree();
        
        _health = _definition.MaxHealth;
    }

    public void TakeDamage(int amount)
    {
        _health = Mathf.Max(0, _health - amount);
        Log.Debug($"'{Name}' took {amount} of damage. Remaining: {_health}.", LogTag);
        if (_health <= 0)
        {
            Log.Debug($"'{Name}' died.", LogTag);
            OnDestroyed?.Invoke();
        }
    }
}