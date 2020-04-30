using Microsoft.Azure.ServiceBus;
using R.BooBus.AzureServiceBus;
using System;
using System.Collections.Generic;
using System.Text;

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
