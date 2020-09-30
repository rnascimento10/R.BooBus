using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace R.BooBus.AzureServiceBus.Extensions
{
    public static class AzureServiceBusExtensions
    {
        private static string _serviceBusConnectionString;

        public static IServiceCollection UseAzureServiceBus(this IServiceCollection services, string serviceBusConnectionString)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            return services;

        }



        public static IServiceCollection WithTopic(this IServiceCollection services, string topicName)
        {
            _serviceBusConnectionString = $"{_serviceBusConnectionString};EntityPath={topicName};";
            return services;
        }


        public static IServiceCollection WithSubscription(this IServiceCollection services, string subscriptionName)
        {
            var _connectionStringBuilder = new ServiceBusConnectionStringBuilder(_serviceBusConnectionString);
            var serviceProvider = services.BuildServiceProvider();

            services.AddTransient<IAzureServiceBus, AzureServiceBusEventBus>(x => new AzureServiceBusEventBus( serviceProvider, new AzureServiceBusPersistentConnection(_connectionStringBuilder), subscriptionName));
            return services;
        }




    }
}
