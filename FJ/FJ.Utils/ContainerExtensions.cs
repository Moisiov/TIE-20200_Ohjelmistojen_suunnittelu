using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.Resolution;

namespace FJ.Utils
{
    public static class ContainerExtensions
    {
        // Store real types for decorated registrations. InterfaceType => TargetTypes in registration order
        private static readonly Dictionary<Type, List<Type>> s_decoratedActualTypes;

        static ContainerExtensions()
        {
            s_decoratedActualTypes = new Dictionary<Type, List<Type>>();
        }
        
        // For unit testing, should be no need to use otherwise. Called with reflection.
        private static void ClearDecoratedTypesBookkeeping()
        {
            s_decoratedActualTypes.Clear();
        }
        
        // Inspiration: https://stackoverflow.com/a/43540162. Modified not to use obsolete Unity.Injection.InjectionFactory
        public static IUnityContainer Decorate<TInterface, TDecorator>(this IUnityContainer container, params InjectionMember[] injectionMembers)
            where TDecorator : class, TInterface
        {
            var uniqueId = Guid.NewGuid().ToString();

            var existingRegistrations = container.Registrations
                .Where(r => r.RegisteredType == typeof(TInterface))
                .ToList();
            if (!existingRegistrations.Any())
            {
                throw new ArgumentException($"No existing registration found for the type {typeof(TInterface)}");
            }

            // Store actual type of decorated implementations on the go
            var latestRegistration = existingRegistrations.Last();
            if (!s_decoratedActualTypes.ContainsKey(typeof(TInterface)))
            {
                s_decoratedActualTypes.Add(typeof(TInterface), new List<Type>());
            }
            
            s_decoratedActualTypes[typeof(TInterface)].Add(latestRegistration.MappedToType);
            
            // Pass topmost lifetime manager forward to decorator
            var lifetimeManager = Activator.CreateInstance(latestRegistration.LifetimeManager.GetType()) as ITypeLifetimeManager;
            
            // 1. Create a wrapper. This is the actual resolution that will be used
            container.RegisterType<TInterface, TDecorator>(uniqueId, lifetimeManager, injectionMembers);

            // 2. Unity comes here to resolve TInterface.
            //     * RegisterFactory overrides existing factory (if any), take that into account.
            container.RegisterFactory<TInterface>((c, t, sName) =>
            {
                var decoratorTypes = s_decoratedActualTypes[typeof(TInterface)];
                
                // 3. We get the decorated class instance TBase
                var baseObj = container.Resolve(s_decoratedActualTypes[typeof(TInterface)].First());
                
                // Check this won't explode resulting StackOverflow
                if (existingRegistrations.Count != decoratorTypes.Count)
                {
                    throw new ResolutionFailedException(typeof(TInterface), sName, "Decorating failed");
                }
                
                // 4. We reference the wrapper TDecorator injecting other Decorators and in the end
                // TBase as TInterface to prevent stack overflow. Constructs in reverse order
                for (var i = 1; i < existingRegistrations.Count; i++)
                {
                    var decoratorName = existingRegistrations[i].Name;
                    var decoratorTargetType = decoratorTypes[i];

                    baseObj = c.Resolve(
                        decoratorTargetType, decoratorName, new DependencyOverride<TInterface>(baseObj));
                }
                
                return c.Resolve<TDecorator>(uniqueId, new DependencyOverride<TInterface>(baseObj));
            });

            return container;
        }
    }
}
