using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace R.BooBus.AzureServiceBus.Extensions
{
    public static class AzureServiceBusExtensions
    {

        public static void UseAzureServiceBus(this IServiceCollection services, string serviceBusConnectionString) 
        {
            var connectionStringBuilder = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);
            services.AddTransient<IAzureServiceBusPersistentConnection<ITopicClient>, AzureServiceBusPersistentConnection>( x => new AzureServiceBusPersistentConnection(connectionStringBuilder));
            services.AddTransient<IAzureServiceBus, AzureServiceBusEventBus>();
        }

       
    }
}
