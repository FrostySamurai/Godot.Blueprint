using System;
using System.Collections.Generic;

namespace Samurai.Application.Events
{
    public delegate void EventCallback<in T>(T evt) where T : IEvent;
    
    internal abstract class EventChannel
    {
        internal abstract Type DataType { get; }

        internal abstract void Unregister(object source);
    }

    internal class EventChannel<T> : EventChannel where T : IEvent
    {
        internal override Type DataType => typeof(T);

        private readonly List<EventCallback<T>> _callbacks = new();
        private readonly Dictionary<object, EventCallback<T>> _callbacksBySource = new();

        private readonly List<EventCallback<T>> _raiseList = new();

        internal void Register(EventCallback<T> callback, object source)
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
            // TODO: optimize this, this is a workaround to avoid changing the list while invoking
            _raiseList.AddRange(_callbacks);
            foreach (var entry in _raiseList)
            {
                entry?.Invoke(@event);
            }
            _raiseList.Clear();
        }

        private void Remove(EventCallback<T> callback, object source)
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