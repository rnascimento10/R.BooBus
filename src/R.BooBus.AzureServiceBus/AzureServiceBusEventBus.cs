using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using R.BooBus.Core;
using System.Text;
using System.Threading.Tasks;

using System;
using Microsoft.Extensions.DependencyInjection;

namespace R.BooBus.AzureServiceBus
{
    public class AzureServiceBusEventBus : IAzureServiceBus 
    {
        private readonly IServiceProvider _provider;
        private readonly IAzureServiceBusPersistentConnection<ITopicClient> _persistentConnection;
        private readonly IEventBusSubscriptionManager _evSubscriptionManager;
        
        public string SubscriptionPropertyForFilterCondition { get; set; }

        public AzureServiceBusEventBus(
            IServiceProvider provider,
            IAzureServiceBusPersistentConnection<ITopicClient> persitentConnection, 
            string subscriptionName
            )
        {
            _provider = provider;
            _persistentConnection = persitentConnection;
            _evSubscriptionManager = new EventBusSubscriptionManager();
            _evSubscriptionManager.OnEventAdded += OnEventAdded;
            _evSubscriptionManager.OnEventRemoved += OnEventRemoved;        
        }

      
        private void OnEventAdded(object sender, string e)
        {
            throw new System.NotImplementedException();
        }

        private void OnEventRemoved(object sender, string e)
        {
            throw new System.NotImplementedException();
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
            var eventName = typeof(TEvent).Name;
            _evSubscriptionManager.AddSubscription<TEvent, THandler>();
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            _evSubscriptionManager.RemoveSubscription<TEvent, THandler>();
        }


        private async Task<bool> Go(string eventName, string message) 
        {
            var processed = false;

            if (_evSubscriptionManager.HasSubscriptions(eventName))
            {

                using (var scope = _provider.CreateScope())
                {
                    var subscriptions = _evSubscriptionManager.GetHandlers(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        await subscription.Handle(message, scope);
                    }
                }

                processed = true;
            }
            return processed;
        }

        
    }
}
