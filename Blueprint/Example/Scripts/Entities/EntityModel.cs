using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Samurai.Application;
using Samurai.Application.Saving;

namespace Samurai.Example.Entities;

[Serializable]
public class EntityModel : ISavable
{
    public readonly Dictionary<string, EntityData> Entities = new();

    #region Lifecycle

    public void OnLoad()
    {
        foreach (var entity in Entities.Values)
        {
            foreach (var data in entity.Components.Values)
            {
                data.OnLoad();
            }
        }
    }

    #endregion Lifecycle

    #region Queries

    public bool TryGetComponent<T>(string entityId, out T data) where T : IComponentData
    {
        data = default;
        if (!Entities.TryGetValue(entityId, out var entry))
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