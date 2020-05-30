using System;
using System.Collections.Generic;

namespace R.BooBus.Core
{
    public interface IEventBusSubscriptionManager
    {
        bool IsEmpty { get; }

        event EventHandler<string> OnEventRemoved;

        event EventHandler<string> OnEventAdded;

        void AddSubscription<TEvent, THandler>()
           where TEvent : Event
           where THandler : IEventHandler<TEvent>;

        void RemoveSubscription<TEvent, THandler>()
           where TEvent : Event
           where THandler : IEventHandler<TEvent>;
       
        bool HasSubscriptions(string eventName);
     
        void Clear();

        IEnumerable<Subscription> GetHandlers(string eventName);

        string GetEventKey<TEvent>();
    }
}
