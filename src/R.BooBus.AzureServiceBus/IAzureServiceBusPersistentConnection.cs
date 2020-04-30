using Microsoft.Azure.ServiceBus;
using System;

namespace R.BooBus.AzureServiceBus
{
    public interface IAzureServiceBusPersistentConnection<T> : IPersistentConection 
        where T : ITopicClient
    {
        ServiceBusConnectionStringBuilder ConnectionStringBuilder { get; }
        T GetModel();
    }
}
