using Samurai.Application.Events;

namespace Samurai.Example.Entities;

public static class EntityEvents
{
    public struct OnEntitySpawned : IEvent
    {
        public string Id;

        public OnEntitySpawned(string id)
        {
            Id = id;
        }
    }
}