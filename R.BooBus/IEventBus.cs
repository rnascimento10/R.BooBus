namespace R.BooBus.Core
{
    public interface IEventBus
    {

        void Publish(IEvent @event);

        void Subscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;

        void Unsubscribe<TEvent, THandler>()
            where TEvent : IEvent
            where THandler : IEventHandler<TEvent>;
           

    }
}
