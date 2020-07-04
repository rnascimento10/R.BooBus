using R.BooBus.Core;

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