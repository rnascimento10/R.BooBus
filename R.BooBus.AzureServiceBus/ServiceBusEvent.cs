using R.BooBus.Core;
using System;

namespace R.BooBus.AzureServiceBus
{
    public class ServiceBusEvent : IEvent
    {

        public ServiceBusEvent()
        {

        }
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreateAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
