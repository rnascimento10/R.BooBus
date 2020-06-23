# R.BooBus [![Build Status](https://travis-ci.com/rnascimento10/R.BooBus.svg?branch=master)](https://travis-ci.com/rnascimento10/R.BooBus)
[![NuGet Package](https://img.shields.io/nuget/v/R.Boobus.AzureServiceBus.svg)](https://www.nuget.org/packages/R.BooBus.AzureServiceBus)

# R.BooBus.AzureServiceBus
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
        //sample from connectionString for Azure service bus: "Endpoint=sb://yourendpoint.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yoursharedaccesskey;"
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
