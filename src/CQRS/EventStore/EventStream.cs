using System.Collections.Generic;

namespace Kofoed.CQRS.EventStore
{
    public class EventStream
    {
        public int Version { get; set; }
        public List<IEvent> Events = new List<IEvent>();
    }
}