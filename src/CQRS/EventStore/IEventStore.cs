using System.Collections.Generic;

namespace Kofoed.CQRS.EventStore
{
    public interface IEventStore
    {
        EventStream LoadEventStream(string streamName);
        void AppendToStream(string streamName, ICollection<IEvent> events, int originalVersion);
    }
}