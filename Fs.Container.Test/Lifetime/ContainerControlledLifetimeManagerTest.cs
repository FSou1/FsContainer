using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Fs.Container.Test.Lifetime {
    [TestClass]
    public class ContainerControlledLifetimeManagerTest {
        [TestMethod]
        public async Task TestContainerControlledLifetimeInstanceAlwaysSame() {
            // Arrange
            var parent = new FsContainer();

            parent
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());

            // Act
            var logger = await parent.ResolveAsync<ILogger>();
            var logger1 = await parent.ResolveAsync<ILogger>();
            var logger2 = await parent.ResolveAsync<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.IsNotNull(logger2);
            Assert.AreSame(logger, logger1);
            Assert.AreSame(logger, logger2);
        }

        [TestMethod]
        public async Task TestMultiThreadContainerControlledLifetimeInstanceAlwaysSame()
        {
            // Arrange
            var parent = new FsContainer();

            parent
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());

            // Act
            var instances = await Task.WhenAll(new[]
            {
                parent.ResolveAsync<ILogger>(),
                parent.ResolveAsync<ILogger>(),
                parent.ResolveAsync<ILogger>()
            });

            var logger = instances[0];
            var logger1 = instances[1];
            var logger2 = instances[2];

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.IsNotNull(logger2);
            Assert.AreSame(logger, logger1);
            Assert.AreSame(logger, logger2);
        }

        [TestMethod]
        public async Task TestParentAndChildResolvesSameContainerControlledInstances()
        {
            // Arrange
            var parent = new FsContainer();

            parent
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());

            var child1 = parent.CreateChildContainer();
            var child2 = parent.CreateChildContainer();

            // Act
            var logger = await parent.ResolveAsync<ILogger>();
            var logger1 = await child1.ResolveAsync<ILogger>();
            var logger2 = await child2.ResolveAsync<ILogger>();

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
            var parent = new FsContainer();

            parent
                .For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());

            var child1 = parent.CreateChildContainer();
            var child2 = parent.CreateChildContainer();

            // Act
            var instances = await Task.WhenAll(new[]
            {
                parent.ResolveAsync<ILogger>(),
                child1.ResolveAsync<ILogger>(),
                child2.ResolveAsync<ILogger>()
            });

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
        public async Task TestMultipleResolvedInstanceDisposeOnlyOnce()
        {
            // Arrange
            var parent = new FsContainer();

            parent
                .For<DisposableObject>()
                .Use<DisposableObject>(new ContainerControlledLifetimeManager());

            // Act
            var o1 = await parent.ResolveAsync<DisposableObject>();
            var o2 = await parent.ResolveAsync<DisposableObject>();
            var o3 = await parent.ResolveAsync<DisposableObject>();

            // Assert
            parent.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
            Assert.IsTrue(o3.WasDisposed);
            Assert.AreEqual(o1.DisposeCount, 1);
            Assert.AreEqual(o2.DisposeCount, 1);
            Assert.AreEqual(o3.DisposeCount, 1);
        }

        [TestMethod]
        public async Task TestMultiThreadMultipleResolvedInstanceDisposeOnlyOnce()
        {
            // Arrange
            var parent = new FsContainer();

            parent
                .For<DisposableObject>()
                .Use<DisposableObject>(new ContainerControlledLifetimeManager());

            // Act
            var instances = await Task.WhenAll(new[]
            {
                parent.ResolveAsync<DisposableObject>(),
                parent.ResolveAsync<DisposableObject>(),
                parent.ResolveAsync<DisposableObject>()
            });

            var o1 = instances[0];
            var o2 = instances[1];
            var o3 = instances[2];

            // Assert
            parent.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
            Assert.IsTrue(o3.WasDisposed);
            Assert.AreEqual(o1.DisposeCount, 1);
            Assert.AreEqual(o2.DisposeCount, 1);
            Assert.AreEqual(o3.DisposeCount, 1);
        }
    }
}
