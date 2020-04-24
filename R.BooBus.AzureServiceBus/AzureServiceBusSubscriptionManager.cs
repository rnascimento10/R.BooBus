using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using R.BooBus.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace R.BooBus.AzureServiceBus
{
    public class AzureServiceBusSubscriptionManager 
    {

        private readonly IDictionary<string, List> _handlers = new Dictionary<string, List>();

        public bool HasHandler => _handlers.Keys.Any();

        public event EventHandler OnEventRemoved;
        public event EventHandler OnEventAdded;

       

        public void AddSubscription<TEvent, TEventHandler>()
            where TEvent : EventMessage
            where TEventHandler : IEventHandler
        {
            AddSubscription(
                typeof(TEventHandler),
                typeof(TEvent).Name,
                typeof(TEvent)
                );
        }

        private void AddSubscription(Type handlerType, string eventName, Type eventType = null)
        {
            if (!HasSubscription(eventName))
            {
                _handlers.Add(eventName, new List());
                OnEventAdded?.Invoke(this, eventName);
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(Subscription.New(
                handlerType, eventType)
                );
        }

        public void RemoveSubscription<TEventHandler>(string eventName)
            where TEventHandler : IDynamicEventHandler
        {
            var handlerToRemove = GetSubscription(eventName);
            RemoveSubscription(eventName, handlerToRemove);
        }


        public void RemoveSubscription<TEvent, TEventHandler>()
            where TEvent : EventMessage
            where TEventHandler : IEventHandler
        {

            var eventName = typeof(TEvent).Name;
            var handlerToRemove = GetSubscription(eventName);
            RemoveSubscription(eventName, handlerToRemove);
        }

        Subscription GetSubscription(string eventName)
        {
            if (!HasSubscription(eventName))
            {
                return null;
            }

            return _handlers[eventName].SingleOrDefault(s => s.HandlerType == typeof(IEventHandler));
        }

        private void RemoveSubscription(
            string eventName,
            Subscription subsToRemove
            )
        {
            if (subsToRemove == null) return;
            _handlers[eventName].Remove(subsToRemove);

            if (_handlers[eventName].Any()) return;

            _handlers.Remove(eventName);
            OnEventRemoved?.Invoke(this, eventName);
        }

        public bool HasSubscription(string eventName) =>
            _handlers.ContainsKey(eventName);

        public IEnumerable GetHandlers()
            where TEvent : EventMessage
        {
            var key = typeof(TEvent).Name;
            return GetHandlers(key);
        }

        public IEnumerable GetHandlers(string eventName) => _handlers[eventName];

        public Type GetEventTypeByName(string eventName) => _handlers[eventName]?.FirstOrDefault(handler => !handler.IsDynamic)?.EventType;
    }

    public class Subscription
    {
        public bool IsDynamic => EventType == null;
        public Type HandlerType { get; }
        public Type EventType { get; }

        private Subscription(Type handlerType, Type eventType = null)
        {
            HandlerType = handlerType;
            EventType = eventType;
        }

        public async Task Handle(string message, ILifetimeScope scope)
        {
            if (IsDynamic)
            {
                dynamic eventData = JObject.Parse(message);
                if (scope.ResolveOptional(HandlerType) is IDynamicEventHandler handler)
                    await handler.Handle(eventData);
            }
            else
            {
                var eventData = JsonConvert.DeserializeObject(message, EventType);
                var handler = scope.ResolveOptional(HandlerType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(EventType);
                await (Task)concreteType.GetMethod("Handle")
                    .Invoke(handler, new[] { eventData });
            }
        }

        public static Subscription New(Type handlerType, Type eventType) =>
            new Subscription(handlerType, eventType);
    }
}
