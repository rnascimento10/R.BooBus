using R.BooBus.AzureServiceBus;
using R.BooBus.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber
{
    public class EventMessageTest : Event
    {
        public EventMessageTest()
        {
        }

        public string Message { get; set; }
    }

}
