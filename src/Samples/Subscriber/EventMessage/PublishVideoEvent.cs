using R.BooBus.AzureServiceBus;
using R.BooBus.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscriber
{
    public class PublishVideoEvent : Event
    {
        public PublishVideoEvent()
        {
        }

        public string Message { get; set; }
    }

}
