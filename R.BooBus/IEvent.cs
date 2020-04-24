using System;

namespace R.BooBus.Core
{
    public interface IEvent
    {
       
        Guid Id { get; set; }

        DateTime CreateAt { get; set; }
    }
}
