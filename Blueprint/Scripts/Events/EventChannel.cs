using System;
using System.Collections.Generic;

namespace Samurai.Application.Events
{
    internal abstract class EventChannel
    {
        internal abstract Type DataType { get; }

        internal abstract void Unregister(object source);
    }

    internal class EventChannel<T> : EventChannel where T : IEvent
    {
        internal override Type DataType => typeof(T);

        private List<Action<T>> _callbacks = new();
        private Dictionary<object, Action<T>> _callbacksBySource = new();

        internal void Register(Action<T> callback, object source)
        {
            if (_callbacksBySource.TryGetValue(source, out var existing))
            {
                Remove(existing, source);
            }
            
            _callbacks.Add(callback);
            _callbacksBySource[source] = callback;
        }

        internal override void Unregister(object source)
        {
            if (_callbacksBySource.TryGetValue(source, out var callback))
            {
                Remove(callback, source);
            }
        }

        internal void Raise(T @event)
        {
            foreach (var entry in _callbacks)
            {
                entry?.Invoke(@event);
            }
        }

        private void Remove(Action<T> callback, object source)
        {
            _callbacksBySource.Remove(source);
            int index = _callbacks.FindIndex(x => x == callback);
            if (index >= 0)
            {
                _callbacks.RemoveAt(index);
            }
        }
    }
}