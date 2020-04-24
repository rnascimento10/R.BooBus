using R.BooBus.AzureServiceBus;

namespace R.BooBus.Tests
{
    internal class FakeEvent : ServiceBusEvent
    {
        public FakeEvent(string topic, string subscription)
            : base (topic, subscription)
        {
        }
    }
}