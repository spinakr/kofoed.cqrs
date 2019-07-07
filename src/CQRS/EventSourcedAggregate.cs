using System.Collections.Generic;

namespace Kofoed.CQRS
{
    public abstract class EventSourcedAggregate
    {
        public string Id { get; set; }

        public List<IEvent> PendingEvents { get; private set; }

        protected EventSourcedAggregate(IEnumerable<IEvent> events) : this()
        {
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }

        protected EventSourcedAggregate()
        {
            PendingEvents = new List<IEvent>();
        }

        protected void Append(IEvent @event)
        {
            PendingEvents.Add(@event);
            Mutate(@event);
        }

        private void Mutate(IEvent @event)
        {
            //Exexute correct method on the aggregate
            ((dynamic)this).When((dynamic)@event);
        }
    }
}