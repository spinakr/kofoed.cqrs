namespace Kofoed.CQRS
{
    public interface IEvent
    {

    }

    public interface IEventHandler<IEvent>
    {
        void Handle(IEvent @event);
    }
}