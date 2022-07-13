using System.Collections.Generic;

namespace PocketCqrs.EventStore
{
    public interface IEventStore
    {
        EventStream LoadEventStream(string streamName);
        IList<IEvent> LoadAllEvents();
        void AppendToStream(string streamName, ICollection<IEvent> events, int originalVersion);
    }
}