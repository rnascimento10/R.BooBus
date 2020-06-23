# BUILD STATUS: [![Build Status](https://travis-ci.com/rnascimento10/R.BooBus.svg?branch=master)](https://travis-ci.com/rnascimento10/R.BooBus)

# R.BooBus.AzureServiceBus
A smart and simple library for pub sub implementtion to Microsoft Azure Service Bus

## How to use this library

In the publish, register the service in net core DI container with the fluent built in api like this:

 [code]
        services
        .UseAzureServiceBus("Endpoint=sb://yourendpoint.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=yoursharedaccesskey;")
        .WithTopic("yourtopic")
        .WithSubscription("yoursubscription");
 [/code]
