using System;
using System.Collections.Generic;
using System.Reflection;
using FJ.Utils;
using NUnit.Framework;
using Unity;

namespace FJ.NUnitTests.CoreTests
{
    [TestFixture]
    public class ContainerDecoratorTests : TestFixtureBase
    {
        private interface IDecorateTestInterface
        {
            public int GetNumber();
        }

        private class TestActualImplementation : IDecorateTestInterface
        {
            public int GetNumber()
            {
                return 100;
            }
        }

        #region TestDecorators

        private class TestDecoratorAddOne : IDecorateTestInterface
        {
            private readonly IDecorateTestInterface m_injected;

            public TestDecoratorAddOne(IDecorateTestInterface actual)
            {
                m_injected = actual;
            }

            public int GetNumber()
            {
                return m_injected.GetNumber() + 1;
            }
        }

        private class TestDecoratorMultiplyByTen : IDecorateTestInterface
        {
            private readonly IDecorateTestInterface m_injected;

            public TestDecoratorMultiplyByTen(IDecorateTestInterface injected)
            {
                m_injected = injected;
            }

            public int GetNumber()
            {
                return m_injected.GetNumber() * 10;
            }
        }

        private class TestDecoratorSubtractTwo : IDecorateTestInterface
        {
            private readonly IDecorateTestInterface m_injected;

            public TestDecoratorSubtractTwo(IDecorateTestInterface injected)
            {
                m_injected = injected;
            }

            public int GetNumber()
            {
                return m_injected.GetNumber() - 2;
            }
        }

        #endregion

        [SetUp]
        public override void OnTestSetup()
        {
            Container = new UnityContainer();
            Container.RegisterType<IDecorateTestInterface, TestActualImplementation>();
            
            // Force-clear decorated types bookkeeping. This is only needed in unit tests.
            var method = typeof(ContainerExtensions)
                .GetMethod("ClearDecoratedTypesBookkeeping", BindingFlags.Static | BindingFlags.NonPublic);
            method?.Invoke(null, null);
        }

        [Test]
        public void TestNoDecorators()
        {
            ResolveAndAssert<TestActualImplementation>(100);
        }

        [Test]
        public void TestOneDecorator()
        {
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            ResolveAndAssert<TestDecoratorAddOne>(101);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            ResolveAndAssert<TestDecoratorSubtractTwo>(98);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            ResolveAndAssert<TestDecoratorMultiplyByTen>(1000);
        }
        
        [Test]
        public void TestMultipleDecorators()
        {
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            ResolveAndAssert<TestDecoratorSubtractTwo>(99);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            ResolveAndAssert<TestDecoratorMultiplyByTen>(990);
        }
        
        [Test]
        public void TestDecoratorOrder()
        {
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            ResolveAndAssert<TestDecoratorMultiplyByTen>(990);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            ResolveAndAssert<TestDecoratorSubtractTwo>(1008);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            ResolveAndAssert<TestDecoratorMultiplyByTen>(990);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            ResolveAndAssert<TestDecoratorAddOne>(981);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            ResolveAndAssert<TestDecoratorSubtractTwo>(999);
            
            OnTestSetup();
            Container.Decorate<IDecorateTestInterface, TestDecoratorMultiplyByTen>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorSubtractTwo>();
            Container.Decorate<IDecorateTestInterface, TestDecoratorAddOne>();
            ResolveAndAssert<TestDecoratorAddOne>(999);
        }

        private void ResolveAndAssert<TTarget>(int expectedNumber)
        {
            var inst = Container.Resolve<IDecorateTestInterface>();
            var number = inst.GetNumber();
            
            Assert.IsTrue(inst is TTarget);
            Assert.AreEqual(expectedNumber, number);
        }
    }
}
