using Microsoft.Extensions.Logging;
using R.BooBus.Core;
using System;
using System.Threading.Tasks;

namespace Subscriber.Handlers
{
    public class PublishedVideoEventHandler : IEventHandler<PublishVideoEvent>
    {
        private ILogger<PublishedVideoEventHandler> _logger;
        private static int count = 0;

        public PublishedVideoEventHandler(ILogger<PublishedVideoEventHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Aqui daremos algum tratamento para a mensagem.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task Handle(PublishVideoEvent @event)
        {
            count++;

            _logger.LogInformation($"I Like this video {@event.Message} at {DateTime.Now} - Counter = {count}");

            return Task.FromResult(true);
        }
    }
}
