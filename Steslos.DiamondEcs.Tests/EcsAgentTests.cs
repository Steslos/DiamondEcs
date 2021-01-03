using Microsoft.VisualStudio.TestTools.UnitTesting;
using Steslos.DiamondEcs.Tests.Models;
using System.Collections.Generic;

namespace Steslos.DiamondEcs.Tests
{
    [TestClass]
    public class EcsAgentTests
    {
        private EcsAgent _ecsAgent = null;

        [TestInitialize]
        public void TestInitialize()
        {
            _ecsAgent = new EcsAgent();
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void CreateEntity_WhenInvoked_ReturnsMaximumAmountOfEntities()
        {
            // Arrange
            var maximumEntities = EcsEntity.MaximumEntities - 1; // minus the singleton entity...

            // Act, Assert
            for (int i = 0; i < maximumEntities; i++)
            {
                Assert.IsNotNull(_ecsAgent.CreateEntity(), "CreateEntity() failed to return an entity instance.");
            }
        }

        [TestMethod]
        public void DestroyEntity_WhenGivenValidEntity_EntityNoLongerExistsInEcsAgent()
        {
            // Arrange
            var entity = _ecsAgent.CreateEntity();
            _ecsAgent.RegisterComponent<TestComponent>();
            _ecsAgent.AddComponent(entity, new TestComponent());

            // Act
            _ecsAgent.DestroyEntity(entity);

            // Assert
#if DEBUG
            try
            {
                _ecsAgent.GetComponent<TestComponent>(entity);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("DebugAssertException", ex.GetType().Name, "Expected exception was not thrown.");
                StringAssert.Contains(ex.Message, "Entity does not have the component requested", "Expected exception message was not found.");
            }
#else
            Assert.ThrowsException<KeyNotFoundException>(() => _ecsAgent.GetComponent<TestComponent>(entity));
#endif
        }

        [TestMethod]
        public void RegisterComponent_AfterRegisteringComponent_AllowsComponentToBeAddedAsSingleton()
        {
            // Arrange
            var expectedSingletonComponent = new TestComponent { TestString = "test" };

            // Act
            _ecsAgent.RegisterComponent<TestComponent>();
            _ecsAgent.AddComponent(expectedSingletonComponent);
            var actualSingletonComponent = _ecsAgent.GetComponent<TestComponent>();

            // Assert
            Assert.AreEqual(expectedSingletonComponent, actualSingletonComponent, "Component added was not the same one returned.");
        }

        [TestMethod]
        public void RegisterComponent_AfterRegisteringComponent_AllowsComponentToBeAddedToEntity()
        {
            // Arrange
            var expectedComponent = new TestComponent { TestString = "test" };
            var entity = _ecsAgent.CreateEntity();

            // Act
            _ecsAgent.RegisterComponent<TestComponent>();
            _ecsAgent.AddComponent(entity, expectedComponent);
            var actualComponent = _ecsAgent.GetComponent<TestComponent>(entity);

            // Assert
            Assert.AreEqual(expectedComponent, actualComponent, "Component added was not the same one returned.");
        }

        [TestMethod]
        public void RegisterSystem_AfterRegisteringSystem_ReturnsSystemInstance()
        {
            // Arrange, Act
            var systemReturned = _ecsAgent.RegisterSystem<TestSystem>();

            // Assert
            Assert.IsNotNull(systemReturned, "Null system was returned.");
            Assert.AreEqual(_ecsAgent, systemReturned.EcsAgent, "EcsAgent in returned system is not the same that created the system.");
        }

        [TestMethod]
        public void RemoveComponent_WhenGivenValidComponent_RemovesComponentFromEntity()
        {
            // Arrange
            var entity = _ecsAgent.CreateEntity();
            _ecsAgent.RegisterComponent<TestComponent>();
            _ecsAgent.AddComponent(entity, new TestComponent());

            // Act
            _ecsAgent.RemoveComponent<TestComponent>(entity);

            // Assert
#if DEBUG
            try
            {
                _ecsAgent.GetComponent<TestComponent>(entity);
            }
            catch (Exception ex)
            {
                Assert.AreEqual("DebugAssertException", ex.GetType().Name, "Expected exception was not thrown.");
                StringAssert.Contains(ex.Message, "Entity does not have the component requested", "Expected exception message was not found.");
            }
#else
            Assert.ThrowsException<KeyNotFoundException>(() => _ecsAgent.GetComponent<TestComponent>(entity));
#endif
        }
    }
}
