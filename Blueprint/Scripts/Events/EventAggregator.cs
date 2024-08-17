using System;
using System.Collections.Generic;

namespace Samurai.Application.Events
{
    public class EventAggregator : IDisposable
    {
        private EventAggregator _parent;
        private List<EventAggregator> _children = new();

        private List<EventChannel> _channels = new();
        private Dictionary<Type, EventChannel> _channelsByEvent = new();

        #region Lifecycle

        public EventAggregator(EventAggregator parent = null)
        {
            _parent = parent;
            _parent?.Add(this);
        }

        public void Dispose()
        {
            _parent?.Remove(this);
        }

        #endregion Lifecycle

        private void Add(EventAggregator child)
        {
            _children.Add(child);
        }

        private void Remove(EventAggregator child)
        {
            _children.Remove(child);
        }

        #region Events

        public void Register<T>(Action<T> callback, object source) where T : IEvent
        {
            GetChannel<T>().Register(callback, source);
        }

        /// <summary>
        /// Unregisters only specific event for passed object. 
        /// </summary>
        public void Unregister<T>(object source) where T : IEvent
        {
            GetChannel<T>().Unregister(source);
        }

        /// <summary>
        /// Unregisters all events for passed object.
        /// </summary>
        public void Unregister(object source)
        {
            foreach (var channel in _channels)
            {
                channel.Unregister(source);
            }
        }

        public void Raise<T>(T @event) where T : IEvent
        {
            GetChannel<T>().Raise(@event);
            foreach (var child in _children)
            {
                child.Raise(@event);
            }
        }

        #endregion Events

        #region Private

        private EventChannel<T> GetChannel<T>() where T : IEvent
        {
            var type = typeof(T);
            if (_channelsByEvent.TryGetValue(type, out var channel))
            {
                return (EventChannel<T>)channel;
            }

            var typedChannel = new EventChannel<T>();
            _channels.Add(typedChannel);
            _channelsByEvent[type] = typedChannel;
            return typedChannel;
        }

        #endregion Private
    }
}