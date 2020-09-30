using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using R.BooBus.AzureServiceBus.Extensions;
using R.BooBus.Core;
using Subscriber.Handlers;

namespace Subscriber
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {

                   
                    services.AddHostedService<Worker>();

                    services.AddSingleton<IEventHandler<PublishVideoEvent>, PublishedVideoEventHandler>();
                    services
                    .UseAzureServiceBus("Endpoint=sb://newsgpsmonitriip.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=LzyI2BapSPyzLU/giGQ/7UTR6s9V+Ip0a5JqrHmZ75c=")
                    .WithTopic("rboobus")
                    .WithSubscription("PublishVideoEvent");

                });
    }
}
