using Microsoft.Azure.ServiceBus;
using System;

namespace R.BooBus.AzureServiceBus
{
    public class AzureServiceBusPersistentConnection : IAzureServiceBusPersistentConnection<ITopicClient>
    {
        protected ITopicClient _azServiceBusTopicClient;

        public AzureServiceBusPersistentConnection(ServiceBusConnectionStringBuilder connectionString)
        {
            ConnectionStringBuilder = connectionString;
            _azServiceBusTopicClient = new TopicClient(connectionString, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ConnectionStringBuilder { get; }
        public bool IsDisposed { get; set; }

        public ITopicClient GetModel()
        {
            if (_azServiceBusTopicClient.IsClosedOrClosing) 
            {
                _azServiceBusTopicClient = new  TopicClient(ConnectionStringBuilder, RetryPolicy.Default);
            }

            return _azServiceBusTopicClient;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            IsDisposed = true;
        }


    }
}
