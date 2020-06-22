using Microsoft.Extensions.Logging;
using R.BooBus.Core;
using System;
using System.Threading.Tasks;

namespace Subscriber.Handlers
{
    public class HelloHandler : IEventHandler<EventMessageTest>
    {
        private ILogger<HelloHandler> _logger;

        public HelloHandler(ILogger<HelloHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(EventMessageTest @event)
        {
            _logger.LogInformation("Obtendo evento {@event} : {time}", @event.Message, DateTimeOffset.Now);

            return Task.FromResult(true);
        }
    }
}
