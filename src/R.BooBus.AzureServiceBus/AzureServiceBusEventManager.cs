using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using R.BooBus.Core;
using System;
using System.Text;

namespace R.BooBus.AzureServiceBus
{
    public class AzureServiceBusEventManager : IAzureServiceBus 
    {
        private readonly string _connectionString;
        private readonly IAzureServiceBusPersistentConnection<ITopicClient> _persistentConnection;
        private readonly int _maxAttempRetry;
        private readonly SubscriptionClient _subscriptionClient;

        public string SubscriptionPropertyForFilterCondition { get; set; }

        public AzureServiceBusEventManager(IAzureServiceBusPersistentConnection<ITopicClient> persitentConnection, 
            string subscriptionName,
            string connectionsString, 
            int maxAttempRetry)
        {
           
            _persistentConnection = persitentConnection;
            _maxAttempRetry = maxAttempRetry > 0 ? maxAttempRetry : 3;

            _subscriptionClient = new SubscriptionClient(persitentConnection.ConnectionStringBuilder, subscriptionName);
        }
        public void Publish(Event @event)
        {
            var evt = (ServiceBusEvent)@event;
            var sbus = _persistentConnection.GetModel();

            var msg = new Message();
            msg.MessageId = evt.Id.ToString();
            msg.UserProperties.Add(this.SubscriptionPropertyForFilterCondition, evt.Subscription);

            var payload = JsonConvert.SerializeObject(msg);
            msg.Body = Encoding.UTF8.GetBytes(payload);

            sbus.SendAsync(msg).GetAwaiter().GetResult();
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            throw new System.NotImplementedException();
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            throw new System.NotImplementedException();
        }


        
    }
}
