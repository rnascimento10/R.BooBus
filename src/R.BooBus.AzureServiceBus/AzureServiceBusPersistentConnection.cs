using Microsoft.Azure.ServiceBus;

namespace R.BooBus.AzureServiceBus
{
    public class AzureServiceBusPersistentConnection : IAzureServiceBusPersistentConnection<ITopicClient>
    {
        protected ITopicClient _azServiceBusTopicClient;
        public bool _isDisposed;

        public AzureServiceBusPersistentConnection(ServiceBusConnectionStringBuilder connectionString)
        {
            ConnectionStringBuilder = connectionString;
            _azServiceBusTopicClient = new TopicClient(connectionString, RetryPolicy.Default);
        }

        public ServiceBusConnectionStringBuilder ConnectionStringBuilder { get; }

      

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
            _isDisposed = true;
        }

    }
}
