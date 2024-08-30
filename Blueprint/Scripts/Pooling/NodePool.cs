using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;
using RedHerring.Extensions.Collections;
using Samurai.Application.Utility;

namespace Samurai.Application.Pooling
{
    public partial class PoolInstance : Node
    {
        public string PrefabPath { get; internal set; }
    }
    
    public static class NodePool
    {
        private const string LogTag = nameof(NodePool);
        private const string PoolInstanceName = nameof(PoolInstance);

        private static CanvasItem _parent2D;
        private static Node3D _parent3D;
        private static ulong _instanceCounter = 0;
        
        private static readonly Dictionary<string, Queue<Node>> _pools = new();
        
        #region Lifecycle

        public static void Init(CanvasItem parent2D, Node3D parent3D)
        {
            if (_parent2D is not null || _parent3D is not null)
            {
                return;
            }

            _parent2D = parent2D;
            _parent2D?.SetProcessMode(Node.ProcessModeEnum.Disabled);
            _parent2D?.SetVisible(false);

            _parent3D = parent3D;
            _parent3D?.SetProcessMode(Node.ProcessModeEnum.Disabled);
            _parent3D?.SetVisible(false);
            
            Log.Debug("Initialized.", LogTag);
        }

        #endregion Lifecycle

        #region Public

        public static T Retrieve<T>(PackedScene prefab, Node parent) where T : Node
        {
            if (prefab is null)
            {
                return null;
            }

            string prefabPath = prefab.GetPath();
            var pool = GetPool(prefabPath);
            T instance = null;
            while (instance is null && !pool.IsNullOrEmpty())
            {
                // TODO: care about possible type mismatch
                var poolItem = pool.Dequeue();
                instance = poolItem as T;
                if (poolItem is not null && instance is null)
                {
                    poolItem.QueueFree();
                }
            }

            if (instance is null)
            {
                instance = prefab.Instantiate<T>();
                instance?.SetName($"{prefab.Name()}_{++_instanceCounter}");
            }

            if (instance is null)
            {
                Log.Error($"Mismatch of passed type '{typeof(T).Name}' and type of prefab on path '{prefabPath}'.", LogTag);
                return null;
            }
            
            var poolInstance = new PoolInstance();
            poolInstance.Name = PoolInstanceName;
            poolInstance.PrefabPath = prefabPath;
            instance.AddChild(poolInstance);

            instance.GetParent()?.RemoveChild(instance);
            parent.AddChild(instance);
            
            return instance;
        }

        public static void Return<T>(IEnumerable<T> instances) where T : Node
        {
            foreach (var entry in instances)
            {
                Return(entry);
            }
        }

        public static void Return<T>(T instance) where T : Node
        {
            if (instance is null)
            {
                return;
            }
            
            var poolInstance = instance.GetNodeOrNull<PoolInstance>(PoolInstanceName);
            if (poolInstance is null)
            {
                instance.QueueFree();
                return;
            }

            var pool = GetPool(poolInstance.PrefabPath);
            Return(instance, pool);
        }

        public static void ReturnChildren<T>(PackedScene prefab, Node parent, bool destroyNonPoolObjects = false) where T : Node
        {
            string path = prefab.GetPath();
            var pool = GetPool(prefab.GetPath());
            foreach (var entry in parent.GetChildren())
            {
                if (entry is not T typedEntry)
                {
                    continue;
                }
                
                var poolInstance = typedEntry.GetNode<PoolInstance>(PoolInstanceName);
                if (poolInstance is null)
                {
                    if (destroyNonPoolObjects)
                    {
                        typedEntry.QueueFree();
                    }
                    
                    continue;
                }

                if (string.Compare(poolInstance.PrefabPath, path, StringComparison.Ordinal) != 0)
                {
                    continue;
                }
                
                Return(typedEntry, pool);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Return<T>(T instance, Queue<Node> pool) where T : Node
        {
            if (instance is null || pool is null)
            {
                return;
            }

            Node parent = instance is Node3D ? _parent3D : _parent2D;
            if (parent is null)
            {
                Log.Error("Parent is missing! Can't return to pool. Destroying instance..", LogTag);
                instance.QueueFree();
                return;
            }
            
            instance.GetParent().RemoveChild(instance);
            parent.AddChild(instance);
            pool.Enqueue(instance);
        }

        #endregion Public

        #region Private

        private static Queue<Node> GetPool(string prefabPath)
        {
            if (!_pools.TryGetValue(prefabPath, out var pool))
            {
                pool = new Queue<Node>();
                _pools[prefabPath] = pool;
            }

            return pool;
        }

        #endregion Private
    }
}