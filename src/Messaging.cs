using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace PocketCqrs
{
    public interface IMessaging
    {
        Result Dispatch(ICommand cmd);
        T Dispatch<T>(IQuery<T> query);
        void Publish(IEvent @event);
    }

    public class Messaging : IMessaging
    {
        private readonly IServiceProvider _provider;

        public Messaging(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Result Dispatch(ICommand command)
        {
            Type type = typeof(ICommandHandler<>);
            Type[] typeArgs = { command.GetType() };
            Type handlerType = type.MakeGenericType(typeArgs);

            using (var scope = _provider.CreateScope())
            {
                dynamic handler = scope.ServiceProvider.GetService(handlerType);
                Result result = handler.Handle((dynamic)command);

                return result;
            }
        }

        public T Dispatch<T>(IQuery<T> query)
        {
            Type type = typeof(IQueryHandler<,>);
            Type[] typeArgs = { query.GetType(), typeof(T) };
            Type handlerType = type.MakeGenericType(typeArgs);

            using (var scope = _provider.CreateScope())
            {
                dynamic handler = scope.ServiceProvider.GetService(handlerType);
                T result = handler.Handle((dynamic)query);
                return result;
            }
        }

        public void Publish(IEvent @event)
        {
            Type type = typeof(IEventHandler<>);
            Type[] typeArgs = { @event.GetType() };
            Type specificHandlerType = type.MakeGenericType(typeArgs);
            Type[] baseEventTypeArgs = { typeof(IEvent) };
            Type baseEventHandlerType = type.MakeGenericType(baseEventTypeArgs);

            using (var scope = _provider.CreateScope())
            {
                IEnumerable<dynamic> handlers = scope.ServiceProvider.GetServices(specificHandlerType);
                IEnumerable<dynamic> baseEventHandlers = scope.ServiceProvider.GetServices(baseEventHandlerType);
                foreach (var handler in handlers)
                {
                    handler.Handle((dynamic)@event);
                }
                foreach (var handler in baseEventHandlers)
                {
                    handler.Handle((dynamic)@event);
                }
            }
        }
    }
}