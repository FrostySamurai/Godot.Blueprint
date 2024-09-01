using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Samurai.Application;
using Samurai.Application.Saving;

namespace Samurai.Example.Entities;

[Serializable]
public class EntityModel : ISavable
{
    public const string LogTag = "Entity";
    
    [JsonIgnore]
    public string Id => "Entity";

    public readonly Dictionary<string, Dictionary<Type, IComponentData>> _entities = new();

    #region Lifecycle

    public void OnLoad()
    {
        foreach (var entity in _entities.Values)
        {
            foreach (var data in entity.Values)
            {
                data.OnLoad();
            }
        }
    }

    #endregion Lifecycle
    
    #region Modification

    public void Register(Entity entity)
    {
        if (_entities.TryGetValue(entity.Id, out var componentsData))
        {
            Log.Debug($"Entity with id '{entity.Id}' already registered. Overwriting..", LogTag);
            componentsData.Clear();
            return;
        }

        componentsData = new Dictionary<Type, IComponentData>();
        _entities[entity.Id] = componentsData;
    }

    public void Unregister(string entityId)
    {
        _entities.Remove(entityId);
    }

    public void AddComponent<T>(string entityId, T data) where T : IComponentData
    {
        if (!_entities.TryGetValue(entityId, out var componentsData))
        {
            Log.Error($"Entity '{entityId}' not registered. Can't add component!", LogTag);
            return;
        }

        var dataType = typeof(T);
        if (componentsData.TryGetValue(dataType, out _))
        {
            Log.Error($"Can't register component of type '{dataType.Name}'! Such component is already added..", LogTag);
            return;
        }

        componentsData[dataType] = data;
    }

    public void RemoveComponent<T>(string entityId) where T : IComponentData
    {
        if (_entities.TryGetValue(entityId, out var components))
        {
            components.Remove(typeof(T));
        }
    }

    #endregion Modification

    #region Queries

    public bool TryGetComponent<T>(string entityId, out T data) where T : IComponentData
    {
        data = default;
        if (!_entities.TryGetValue(entityId, out var componentsData))
        {
            return false;
        }

        if (!componentsData.TryGetValue(typeof(T), out var c))
        {
            return false;
        }

        data = (T)c;
        return true;
    }

    #endregion Queries
}