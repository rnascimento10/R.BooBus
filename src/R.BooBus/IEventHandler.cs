using System.Threading.Tasks;

namespace R.BooBus.Core
{
    
    public interface IEventHandler<in TEvent> : IEventHandler
       where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IEventHandler
    {
    }
}