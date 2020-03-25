using System;
using FJ.Utils;
using Moq;
using NUnit.Framework;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace FJ.NUnitTests
{
    /// <summary>
    /// Modified from https://duanenewman.net/blog/post/better-unit-testing-with-ioc-di-and-mocking/
    /// </summary>
    public abstract class TestFixtureBase
    {
        protected readonly IUnityContainer Container;
        protected LifetimeResetter Resetter { get; set; }

        protected TestFixtureBase()
        {
            Container = new UnityContainer();
            Resetter = new LifetimeResetter();
        }

        [SetUp]
        public void OnTestSetup()
        {
            Resetter.Reset();
        }
        
        protected void RegisterResettableType<T>(params InjectionMember[] injectionMembers)
        {
            Container.RegisterType<T>(new ResettableLifetimeManager(Resetter), injectionMembers);
        }
        
        protected void RegisterResettableType<T>(Func<Action<Mock<T>>> onCreatedCallbackFactory)
            where T : class
        {
            Container.RegisterFactory<T>(
                c => CreateMockInstance(onCreatedCallbackFactory),
                new ResettableLifetimeManager(Resetter));
        }

        protected void RegisterResettableDecorator<TInterface, TDecorator>(params InjectionMember[] injectionMembers)
            where TDecorator : class, TInterface
        {
            Container.Decorate<TInterface, TDecorator>(new ResettableLifetimeManager(Resetter));
        }

        private static T CreateMockInstance<T>(Func<Action<Mock<T>>> onCreatedCallbackFactory)
            where T : class
        {
            var mock = new Mock<T>();
            var onCreatedCallback = onCreatedCallbackFactory();
            onCreatedCallback?.Invoke(mock);
            return mock.Object;
        }
        
        #region Internal use
        protected class LifetimeResetter
        {
            public event EventHandler<EventArgs> OnReset;

            public void Reset()
            {
                OnReset?.Invoke(this, EventArgs.Empty);
            }
        }

        protected class ResettableLifetimeManager : LifetimeManager, ITypeLifetimeManager, IFactoryLifetimeManager
        {
            private object m_instance;
            
            public ResettableLifetimeManager(LifetimeResetter lifetimeResetter)
            {
                lifetimeResetter.OnReset += (o, args) => m_instance = null;
            }

            public override object GetValue(ILifetimeContainer container = null)
            {
                return m_instance;
            }

            public override void SetValue(object newValue, ILifetimeContainer container = null)
            {
                m_instance = newValue;
            }

            public override void RemoveValue(ILifetimeContainer container = null)
            {
                m_instance = null;
            }

            protected override LifetimeManager OnCreateLifetimeManager()
            {
                return this;
            }
        }
        #endregion
    }
}
