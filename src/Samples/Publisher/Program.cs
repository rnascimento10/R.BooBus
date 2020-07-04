using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using R.BooBus.AzureServiceBus.Extensions;

namespace Publisher
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
                    services
                    .UseAzureServiceBus("Endpoint=sb://yourendpoint.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yoursharedaccesskey;")
                    .WithTopic("yourtopic")
                    .WithSubscription("yoursubscription");


                });
    }
}
