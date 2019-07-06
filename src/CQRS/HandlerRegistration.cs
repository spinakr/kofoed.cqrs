using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace CQRS
{
    public static class HandlerRegistration
    {
        public static void AddHandlers(this IServiceCollection services, Assembly assemblyToScann)
        {
            List<Type> handlerTypes = assemblyToScann.GetTypes()
                .Where(x => x.GetInterfaces().Any(y => IsHandlerInterface(y)))
                // .Where(x => x.Name.EndsWith("Handler") || x.Name.EndsWith("Projecion"))
                .ToList();

            foreach (Type type in handlerTypes)
            {
                List<Type> interfaceTypes = type.GetInterfaces().Where(y => IsHandlerInterface(y)).ToList();
                interfaceTypes.Select(t => services.AddTransient(t, type)).ToList();
            }
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            Type typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommandHandler<>) ||
                   typeDefinition == typeof(IEventHandler<>) ||
                   typeDefinition == typeof(IQueryHandler<,>);
        }
    }
}