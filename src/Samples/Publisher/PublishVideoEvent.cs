using R.BooBus.Core;

namespace Publisher
{
    public class PublishVideoEvent : Event
    {
        public PublishVideoEvent()
        {
        }

        public string Message { get; set; }
    }
}