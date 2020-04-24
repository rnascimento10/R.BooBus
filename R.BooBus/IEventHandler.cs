using System.Threading.Tasks;

namespace R.BooBus.Core
{
    
    public interface IEventHandler<in TEvent> : IEventHandler
       where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {
    }
}