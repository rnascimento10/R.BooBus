using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using R.BooBus.Core;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace R.BooBus.AzureServiceBus
{
    public class AzureServiceBusEventBus : IAzureServiceBus
    {
       
        private readonly ILogger<AzureServiceBusEventBus> _logger;
        private readonly IServiceProvider _provider;
        private readonly IAzureServiceBusPersistentConnection<ITopicClient> _persistentConnection;
        private readonly IEventBusSubscriptionManager _evSubscriptionManager;
        private readonly SubscriptionClient _subscriptionClient;

       
        public AzureServiceBusEventBus(
            IServiceProvider provider,
            IAzureServiceBusPersistentConnection<ITopicClient> persitentConnection,
            string subscriptionName
            )
        {
            var loggerFactory = new LoggerFactory();
            _logger = loggerFactory.CreateLogger<AzureServiceBusEventBus>();

            _provider = provider;
            _persistentConnection = persitentConnection;

            _evSubscriptionManager = new EventBusSubscriptionManager();
            _evSubscriptionManager.OnEventAdded += OnEventAdded;
            _evSubscriptionManager.OnEventRemoved += OnEventRemoved;

            _subscriptionClient = new SubscriptionClient(persitentConnection.ConnectionStringBuilder, subscriptionName);
            _subscriptionClient.PrefetchCount = 300;

            RegisterMessageListener();

        }

        public void Publish(Event @event)
        {
            var evt = @event;
            var sbus = _persistentConnection.GetModel();

            var msg = new Message();
            msg.Label = evt.GetType().Name;
            msg.MessageId = evt.Id.ToString();
            msg.UserProperties.Add("Label", msg.Label);

            var payload = JsonConvert.SerializeObject(evt);
            msg.Body = Encoding.UTF8.GetBytes(payload);

            sbus.SendAsync(msg).GetAwaiter().GetResult();
        }

        public void Subscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {

            _evSubscriptionManager.AddSubscription<TEvent, THandler>();
        }

        public void Unsubscribe<TEvent, THandler>()
            where TEvent : Event
            where THandler : IEventHandler<TEvent>
        {
            _evSubscriptionManager.RemoveSubscription<TEvent, THandler>();
        }


        private async Task<bool> ProcessEvent(string eventName, string message)
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

        private void RegisterMessageListener()
        {
            _subscriptionClient.RegisterMessageHandler(async (message, token) =>
               {
                   var eventName = message.Label;
                   var messageData = Encoding.UTF8.GetString(message.Body);


                   if (await ProcessEvent(eventName, messageData))
                   {
                       await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                   }
               },
               new MessageHandlerOptions(ExceptionReceivedHandler) 
               { 
                   MaxConcurrentCalls = 20, 
                   AutoComplete = false 
               });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError($"Exception in message handling: {exceptionReceivedEventArgs.Exception}.");

            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            _logger.LogInformation("Exception context for troubleshooting:");
            _logger.LogInformation($"- Endpoint: {context.Endpoint}");
            _logger.LogInformation($"- Entity Path: {context.EntityPath}");
            _logger.LogInformation($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        private void OnEventAdded(object sender, string e)
        {
            var containsKey = _evSubscriptionManager.HasSubscriptions(e);
            if (!containsKey)
            {
                try
                {
                    _subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = e },
                        Name = e
                    }).GetAwaiter().GetResult();
                }
                catch (ServiceBusException ex)
                {
                    _logger.LogCritical("Failure OnEventAdded: ", ex);
                    throw;
                }
            }
        }

        private void OnEventRemoved(object sender, string e)
        {
            try
            {
                _subscriptionClient
                 .RemoveRuleAsync(e)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException ex)
            {
                _logger.LogCritical("Failure onEventRemoved: ", ex);
                throw;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            _evSubscriptionManager.Clear();
        }
    }
}
