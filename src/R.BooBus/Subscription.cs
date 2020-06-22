using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace R.BooBus.Core
{
    public class Subscription
    {
       
        public Type HandlerType { get; }
        public Type EventType { get; }

        public Subscription(Type handlerType, Type eventType)
        {
            HandlerType = handlerType;
            EventType = eventType;
        }

        public async Task Handle(string message, IServiceScope scope)
        {
            var eventData = JsonConvert.DeserializeObject(message, EventType);
            var concreteType = typeof(IEventHandler<>).MakeGenericType(EventType);
            var handler = scope.ServiceProvider.GetRequiredService(concreteType);
            
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new[] { eventData });

        }

        public static Subscription Create( Type handlerType, Type eventType) => new Subscription(handlerType, eventType);

    }
}
