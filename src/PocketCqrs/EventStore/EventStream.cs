using System.Collections.Generic;

namespace PocketCqrs.EventStore
{
    public class EventStream
    {
        public int Version { get; set; }
        public List<IEvent> Events = new List<IEvent>();
    }
}