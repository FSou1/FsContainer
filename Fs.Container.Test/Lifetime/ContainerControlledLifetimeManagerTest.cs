using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Fs.Container.Test.Lifetime {
    [TestClass]
    public class ContainerControlledLifetimeManagerTest {
        private IFsContainer child1;
        private IFsContainer child2;
        private IFsContainer parent;

        public ContainerControlledLifetimeManagerTest()
        {
            parent = new FsContainer();
            parent.For<ILogger>()
                .Use<Logger>(new ContainerControlledLifetimeManager());
            parent.For<DisposableObject>()
                .Use<DisposableObject>(new ContainerControlledLifetimeManager());
            child1 = parent.CreateChildContainer();
            child2 = parent.CreateChildContainer();
        }

        [TestMethod]
        public async Task TestContainerControlledLifetimeInstanceAlwaysSame() {
            // Arrange
            var logger = await parent.ResolveAsync<ILogger>();
            var logger1 = await parent.ResolveAsync<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreSame(logger, logger1);
        }

        [TestMethod]
        public async Task ParentAndChildResolvesSameContainerControlledInstances()
        {
            // Arrange
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
        public async Task MultipleResolvedInstanceDisposeOnlyOnce()
        {
            var o1 = await parent.ResolveAsync<DisposableObject>();
            var o2 = await parent.ResolveAsync<DisposableObject>();

            parent.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
            Assert.AreEqual(o1.DisposeCount, 1);
            Assert.AreEqual(o2.DisposeCount, 1);
        }
    }
}
