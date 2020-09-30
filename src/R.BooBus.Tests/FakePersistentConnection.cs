using Microsoft.Azure.ServiceBus;
using R.BooBus.AzureServiceBus;

namespace R.BooBus.Tests
{
    public class FakePersistentConnection : AzureServiceBusPersistentConnection
    {
        public FakePersistentConnection(ServiceBusConnectionStringBuilder connectionString) 
            : base(connectionString)
        {
        }

        public ITopicClient ServiceBusTopicClient 
        { 
            get { return _azServiceBusTopicClient; }
            set { _azServiceBusTopicClient = value; }
        }
    }
}
