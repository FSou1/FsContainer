using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace Fs.Container.Test.Lifetime {
    [TestClass]
    public class TransientLifetimeManagerTest {
        private FsContainer parent;

        public TransientLifetimeManagerTest()
        {
            parent = new FsContainer();

            parent.For<ILogger>()
                .Use<Logger>(new TransientLifetimeManager());
            parent.For<DisposableObject>()
                .Use<DisposableObject>(new TransientLifetimeManager());
        }

        [TestMethod]
        public async Task TestTransientLifetimeInstanceAlwaysDifferent() {
            // Arrange
            var logger = await parent.ResolveAsync<ILogger>();
            var logger1 = await parent.ResolveAsync<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreNotSame(logger, logger1);
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
