using Microsoft.Azure.ServiceBus;
using System;

namespace R.BooBus.AzureServiceBus
{
    public interface IAzureServiceBusPersistentConnection<T> : IPersistentConnection<T>, IDisposable
        where T : ITopicClient
    {
        ServiceBusConnectionStringBuilder ConnectionStringBuilder { get; }
 
    }
}
