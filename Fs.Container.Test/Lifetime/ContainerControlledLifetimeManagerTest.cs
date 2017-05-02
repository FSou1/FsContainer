using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Lifetime {
    [TestClass]
    public class ContainerControlledLifetimeManagerTest {
        private FsContainer child1;
        private FsContainer child2;
        private FsContainer parent;

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
        public void TestContainerControlledLifetimeInstanceAlwaysSame() {
            // Arrange
            var logger = parent.Resolve<ILogger>();
            var logger1 = parent.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreSame(logger, logger1);
        }

        [TestMethod]
        public void ParentAndChildResolvesSameContainerControlledInstances()
        {
            // Arrange
            var logger = parent.Resolve<ILogger>();
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
        public void MultipleResolvedInstanceDisposeOnlyOnce()
        {
            var o1 = parent.Resolve<DisposableObject>();
            var o2 = parent.Resolve<DisposableObject>();

            parent.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
            Assert.AreEqual(o1.DisposeCount, 1);
            Assert.AreEqual(o2.DisposeCount, 1);
        }
    }
}
