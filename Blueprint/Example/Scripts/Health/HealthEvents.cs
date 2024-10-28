using Samurai.Application.Events;

namespace Samurai.Example.Health;

public static class HealthEvents
{
    public struct OnDamageTaken : IEvent
    {
        public int Old;
        public int Current;
        public int Amount;

        public OnDamageTaken(int old, int current, int amount)
        {
            Old = old;
            Current = current;
            Amount = amount;
        }
    }

    public struct OnDestroyed : IEvent
    {
        public string EntityId;

        public OnDestroyed(string entityId)
        {
            EntityId = entityId;
        }

        public bool IsMatch(string entityId) => EntityId == entityId;
    }
}