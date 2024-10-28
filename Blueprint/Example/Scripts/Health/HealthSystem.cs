using Godot;
using Samurai.Application;
using Samurai.Example.Entities;
using Samurai.Example.Health.Data;

namespace Samurai.Example.Health;

public static class HealthSystem
{
    public const string LogTag = nameof(HealthSystem);
    
    public static void DealDamageTo(string entityId, int amount)
    {
        if (!Session.Get<EntityModel>().TryGetComponent(entityId, out HealthData health))
        {
            return;
        }

        int old = health.Current;
        health.Current = Mathf.Max(0, health.Current - amount);
        Log.Debug($"'{entityId}' took {amount} of damage. Remaining: {health.Current}.", LogTag);
        Session.Events.Raise(new HealthEvents.OnDamageTaken(old, health.Current, amount));
        if (health.Current <= 0)
        {
            Log.Debug($"'{entityId}' died.", LogTag);
            Session.Events.Raise(new HealthEvents.OnDestroyed(entityId));
        }
    }
}