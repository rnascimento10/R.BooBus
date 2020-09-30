using System;

namespace R.BooBus.Core
{
    public abstract class Event
    {

        protected Event()
        {
            Id = Guid.NewGuid();
            CreateAt = DateTime.Now;
        }
       
        public Guid Id { get; set; }

        public DateTime CreateAt { get; set; }

    }
}
