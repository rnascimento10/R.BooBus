using System;
using System.Collections.Generic;
using System.Linq;

namespace R.BooBus.Core
{
    public class EventBusSubscriptionManager : IEventBusSubscriptionManager
    {

        private readonly IDictionary<string, List<Subscription>> _eventHandlers;


        public bool IsEmpty => _eventHandlers.Keys.Any();

        public event EventHandler<string> OnEventRemoved;

        public event EventHandler<string> OnEventAdded;

        public EventBusSubscriptionManager()
        {
            _eventHandlers = new Dictionary<string, List<Subscription>>();

        }

        public void AddSubscription<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            AddSubscription(typeof(THandler), GetEventKey<TEvent>(), typeof(TEvent));
        }

        public void Clear() => _eventHandlers.Clear();

        public string GetEventKey<TEvent>() => typeof(TEvent).Name;

        public IEnumerable<Subscription> GetHandlers<TEvent>() where TEvent : Event
        {
            var eventName = GetEventKey<TEvent>();
            return GetHandlers(eventName);
        }

        public bool HasSubscriptions(string eventName) => _eventHandlers.ContainsKey(eventName);

        public void RemoveSubscription<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            var eventName = GetEventKey<TEvent>();
            var subsToRemove = GetSubscriptionToRemove(eventName, typeof(THandler));

            if (subsToRemove != null)
            {
                RemoveSubscription(eventName, subsToRemove);
            }
        }


        void AddSubscription(Type handlerType, string eventName, Type eventType = null)
        {
            if (!HasSubscriptions(eventName))
            {
                _eventHandlers.Add(eventName, new List<Subscription>());
                OnEventAdded?.Invoke(this, eventName);
            }

            if (_eventHandlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler {handlerType.Name} already register for '{eventName}'", nameof(handlerType));
            }
            _eventHandlers[eventName].Add(Subscription.Create(handlerType, eventType));
        }

        void RemoveSubscription(string eventName, Subscription subsToRemove)
        {
            if (subsToRemove != null)
            {
                _eventHandlers[eventName].Remove(subsToRemove);
                if (!_eventHandlers[eventName].Any())
                {
                    _eventHandlers.Remove(eventName);
                    OnEventRemoved?.Invoke(this, eventName);
                }
            }

        }

        Subscription GetSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptions(eventName))
            {
                return null;
            }

            return _eventHandlers[eventName].SingleOrDefault(s => s.HandlerType == handlerType);
        }

        public IEnumerable<Subscription> GetHandlers(string eventName) => _eventHandlers[eventName];
    }
}
