# R.BooBus.AzureServiceBus [![Build Status](https://travis-ci.com/rnascimento10/R.BooBus.svg?branch=master)](https://travis-ci.com/rnascimento10/R.BooBus) [![NuGet Package](https://img.shields.io/nuget/v/R.Boobus.AzureServiceBus.svg)](https://www.nuget.org/packages/R.BooBus.AzureServiceBus)

A smart and simple library for pub sub implementtion to Microsoft Azure Service Bus

### Package Manager Console

```
Install-Package R.BooBus.AzureServiceBus
```

### .NET Core CLI

```
dotnet add package R.BooBus.AzureServiceBus
```

## Usage

1 - In the publish process, register the service in net core DI container with the fluent built in api like this:

```csharp
      //A instace of ServiceCollection 
        services
        //sample from connectionString for Azure service bus:
       // "Endpoint=sb://yourendpoint.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yoursharedaccesskey;"
     .UseAzureServiceBus("YourConnectionString")
     //Configuring your topic name
        .WithTopic("yourtopic")
        //Configuring your subscription name
        .WithSubscription("yoursubscription");

```
2 - Create a custom event that inherits from the Event Class of the R.Boobus.Core namespace
```csharp

 public class MyEvent : Event
 {
 }
```
3 - We are now ready to publish the event on the bus.

```csharp
 public class SomeClass 
 {
        private readonly IAzureServiceBus _azureServiceBus;

        public SomeClass(IAzureServiceBus azureServiceBus)
        {
            _azureServiceBus = azureServiceBus;
        }

        protected async Task runAsync()
        {
             var message = new MyEvent();             
             _azureServiceBus.Publish(message);
        }
    }
```
5 - In the subscriber process, register the service in net core DI container with the fluent built in api like this:

```csharp
      //A instace of ServiceCollection 
        services
        //sample from connectionString for Azure service bus:
       // "Endpoint=sb://yourendpoint.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yoursharedaccesskey;"
     .UseAzureServiceBus("YourConnectionString")
     //Configuring your topic name
        .WithTopic("yourtopic")
        //Configuring your subscription name
        .WithSubscription("yoursubscription");
```

6 - Subscribe to start receiving your message or unsubscribe to stop receiving then.

```csharp
    public class Worker : BackgroundService
    {
        private readonly IAzureServiceBus _azureServiceBus;

        public Worker(IAzureServiceBus azureServiceBus)
        {
            _azureServiceBus = azureServiceBus;
        }

       
        public override Task StartAsync(CancellationToken cancellationToken)
        {

            _azureServiceBus.Subscribe<EventMessageTest, HelloHandler>();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _azureServiceBus.Unsubscribe<EventMessageTest, HelloHandler>();
            return base.StopAsync(cancellationToken);
        }
    }
```


7 - Create the same event created on publish process that inherits from the Event Class of the R.Boobus.Core namespace
```csharp

 public class MyEvent : Event
 {
 }
```
8 - Create the event handler that implement IEventHandler<TEvent> from the R.Boobus.Core namespace
```csharp
 public class HelloHandler : IEventHandler<MyEvent>
    {
        private ILogger<HelloHandler> _logger;

        public HelloHandler(ILogger<HelloHandler> logger)
        {
            _logger = logger;
        }
        public Task Handle(MyEvent @event)
        {
            _logger.LogInformation("Get Event from  {@event} at {time}", @event.ToString(), DateTimeOffset.Now);

            return Task.FromResult(true);
        }
    }
```


