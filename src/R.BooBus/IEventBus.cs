using System;

namespace R.BooBus.Core
{
    public interface IEventBus : IDisposable
    {

        void Publish(Event @event);

        void Subscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;

        void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>;
           

    }
}
