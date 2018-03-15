using System.Threading.Tasks;
using Fs.Container.Core;
using Fs.Container.Core.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Core.Lifetime {
    [TestClass]
    public class TransientLifetimeManagerTest {
        private FsContainer container;

        public TransientLifetimeManagerTest()
        {
            container = new FsContainer();

            container.For<ILogger>()
                .Use<Logger>(new TransientLifetimeManager());
            container.For<DisposableObject>()
                .Use<DisposableObject>(new TransientLifetimeManager());
        }

        [TestMethod]
        public void TestTransientLifetimeInstanceAlwaysDifferent() {
            // Arrange
            var logger = container.Resolve<ILogger>();
            var logger1 = container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreNotSame(logger, logger1);
        }

        [TestMethod]
        public async Task TestMultiThreadTransientLifetimeInstanceAlwaysDifferent()
        {
            // Arrange
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
            Assert.AreNotSame(logger, logger1);
        }

        [TestMethod]
        public void MultipleResolvedInstanceDisposeOnlyOnce()
        {
            var o1 = container.Resolve<DisposableObject>();
            var o2 = container.Resolve<DisposableObject>();

            container.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
            Assert.AreEqual(o1.DisposeCount, 1);
            Assert.AreEqual(o2.DisposeCount, 1);
        }
    }
}
