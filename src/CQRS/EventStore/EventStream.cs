using System.Collections.Generic;
using CQRS;

namespace BankAccounts.CQRS.EventStore
{
    public class EventStream
    {
        public int Version { get; set; }
        public List<IEvent> Events = new List<IEvent>();
    }
}