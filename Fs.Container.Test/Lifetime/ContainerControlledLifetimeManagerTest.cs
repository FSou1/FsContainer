using System.Threading;
using System.Threading.Tasks;
using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Lifetime {
    [TestClass]
    public class ContainerControlledLifetimeManagerTest {
        [TestMethod]
        public void TestContainerControlledLifetimeInstanceAlwaysSame() {
            var container = new FsContainer();

            container.For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());

            // Arrange
            var logger = container.Resolve<ILogger>();
            var logger1 = container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreSame(logger, logger1);
        }

        [TestMethod]
        public async Task TestMultiThreadContainerControlledLifetimeInstanceAlwaysSame()
        {
            // Arrange
            var container = new FsContainer();
            container
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());

            // Act
            var instances = await Task.WhenAll(
                Task.Run(() => {
                    Task.Delay(10);
                    return container.Resolve<ILogger>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return container.Resolve<ILogger>();
                })
            );

            var logger = instances[0];
            var logger1 = instances[1];

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreSame(logger, logger1);
        }

        [TestMethod]
        public void TestParentAndChildResolvesSameContainerControlledInstances()
        {
            // Arrange
            var container = new FsContainer();
            container
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());
            var child1 = container.CreateChildContainer();
            var child2 = container.CreateChildContainer();

            // Arrange
            var logger = container.Resolve<ILogger>();
            var logger1 = child1.Resolve<ILogger>();
            var logger2 = child2.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.IsNotNull(logger2);
            Assert.AreSame(logger, logger1);
            Assert.AreSame(logger, logger2);
            Assert.AreSame(logger1, logger2);            
        }

        [TestMethod]
        public async Task TestMultiThreadParentAndChildResolvesSameContainerControlledInstances()
        {
            // Arrange
            var container = new FsContainer();
            container
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());
            var child1 = container.CreateChildContainer();
            var child2 = container.CreateChildContainer();

            // Arrange
            var instances = await Task.WhenAll(
                Task.Run(() => {
                    Task.Delay(10);
                    return container.Resolve<ILogger>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return child1.Resolve<ILogger>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return child2.Resolve<ILogger>();
                })
            );

            var logger = instances[0];
            var logger1 = instances[1];
            var logger2 = instances[2];

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.IsNotNull(logger2);
            Assert.AreSame(logger, logger1);
            Assert.AreSame(logger, logger2);
            Assert.AreSame(logger1, logger2);
        }

        [TestMethod]
        public void TestMultipleResolvedInstanceDisposeOnlyOnce()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<DisposableObject>()
                .Use<DisposableObject>(new ContainerControlledLifetimeManager());

            // Act
            var o1 = container.Resolve<DisposableObject>();
            var o2 = container.Resolve<DisposableObject>();

            // Assert
            container.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
            Assert.AreEqual(o1.DisposeCount, 1);
            Assert.AreEqual(o2.DisposeCount, 1);
        }
    }
}
