using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Samurai.Application.Saving;

namespace Samurai.Example.Entities;

[Serializable]
public class EntityModel : ISavable
{
    private readonly List<EntityData> _entities = new();
    
    [JsonIgnore]
    private readonly Dictionary<string, EntityData> _entitiesById = new();

    #region Lifecycle

    public void OnLoad()
    {
        foreach (var entity in _entities)
        {
            foreach (var data in entity.Components.Values)
            {
                data.OnLoad();
            }
        }
    }

    #endregion Lifecycle

    #region Manipulation

    public void Add(EntityData entity)
    {
        if (entity is null)
        {
            return;
        }

        if (_entitiesById.TryAdd(entity.Id, entity))
        {
            _entities.Add(entity);
        }
    }

    public void Remove(string id)
    {
        if (_entitiesById.TryGetValue(id, out var entity))
        {
            _entities.Remove(entity);
        }
    }

    #endregion Manipulation

    #region Queries

    public IEnumerable<EntityData> GetAll()
    {
        return _entities;
    }

    public IEnumerable<T> GetAllComponents<T>() where T : IComponentData
    {
        var componentType = typeof(T);
        foreach (var entry in _entities)
        {
            if (entry.Components.TryGetValue(componentType, out var component))
            {
                yield return (T)component;
            }
        }
    }

    public bool TryGetComponent<T>(string entityId, out T data) where T : IComponentData
    {
        data = default;
        if (!_entitiesById.TryGetValue(entityId, out var entry))
        {
            return false;
        }

        if (!entry.Components.TryGetValue(typeof(T), out var c))
        {
            return false;
        }

        data = (T)c;
        return true;
    }

    #endregion Queries
}