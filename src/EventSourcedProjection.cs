using System.Collections.Generic;

namespace PocketCqrs
{
    public abstract class EventSourcedProjection
    {
        public string Id { get; }

        protected EventSourcedProjection(IEnumerable<IEvent> events) : this()
        {
            foreach (var @event in events)
            {
                Mutate(@event);
            }
        }

        protected EventSourcedProjection()
        {
        }

        protected void Mutate(IEvent @event)
        {
            //Exexute correct method on the aggregate
            ((dynamic)this).When((dynamic)@event);
        }
    }
}