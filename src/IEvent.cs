namespace PocketCqrs
{
    public interface IEvent
    {

    }

    public interface IEventHandler<IEvent>
    {
        void Handle(IEvent @event);
    }
}