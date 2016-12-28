using Fs.Container.Lifetime;
using Fs.Container.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Test {
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
        public void TestTransientLifetimeInstanceAlwaysDifferent() {
            // Arrange
            var logger = parent.Resolve<ILogger>();
            var logger1 = parent.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreNotSame(logger, logger1);
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
