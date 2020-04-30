using R.BooBus.Core;

namespace R.BooBus.AzureServiceBus
{
    public interface IAzureServiceBus : IEventBus
    {
        string SubscriptionPropertyForFilterCondition { get; set; }
    }
}