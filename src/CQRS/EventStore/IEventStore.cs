using System;
using System.Collections.Generic;
using CQRS;

namespace BankAccounts.CQRS.EventStore
{
    public interface IEventStore
    {
        EventStream LoadEventStream(string streamName);
        void AppendToStream(string streamName, ICollection<IEvent> events, int originalVersion);
    }
}