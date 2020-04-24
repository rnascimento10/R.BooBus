using R.BooBus.Core;
using System;

namespace R.BooBus.AzureServiceBus
{
    public class ServiceBusEvent : IEvent
    {

        public ServiceBusEvent(string topic, string subscription)
        {

            Id = Guid.NewGuid();
            CreateAt = DateTime.Now;

            if (string.IsNullOrWhiteSpace(topic)) throw new ArgumentNullException("Topic should not be null or empty");
            if (string.IsNullOrWhiteSpace(subscription)) throw new ArgumentNullException("Subscription should not be null or empty");

            Topic = topic;
            Subscription = subscription;
        }

        public Guid Id { get; set; }
        public DateTime CreateAt { get; set; }
        public string Topic { get; protected set; }
        public string Subscription { get; protected set; }
    }
}
