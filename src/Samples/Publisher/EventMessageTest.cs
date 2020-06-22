using R.BooBus.Core;
using System.Net.Sockets;

namespace Publisher
{
    public class EventMessageTest : Event
    {
        public EventMessageTest()
        {
        }

        public string Message { get; set; }
    }
}