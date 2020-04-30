using R.BooBus.Core;
using System;


namespace R.BooBus.AzureServiceBus
{
    public class ServiceBusEvent : Event
    {

        public ServiceBusEvent(string topic, string subscription)
        {

            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentNullException("Topic should not be null or empty");
            if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentNullException("Subscription should not be null or empty");

            Topic = topic;
            Subscription = subscription;
        }

       
        public string Topic { get; protected set; }
        public string Subscription { get; protected set; }
       
    }
}
