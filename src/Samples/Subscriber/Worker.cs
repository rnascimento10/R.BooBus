using Microsoft.Extensions.Hosting;
using R.BooBus.AzureServiceBus;
using Subscriber.Handlers;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    public class Worker : BackgroundService
    {
        private IAzureServiceBus _azureServiceBus;

        public Worker(IAzureServiceBus azureServiceBus)
        {
            _azureServiceBus = azureServiceBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _azureServiceBus.Subscribe<PublishVideoEvent, PublishedVideoEventHandler>();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _azureServiceBus.Unsubscribe<PublishVideoEvent, PublishedVideoEventHandler>();
            return base.StopAsync(cancellationToken);
        }
    }
}
