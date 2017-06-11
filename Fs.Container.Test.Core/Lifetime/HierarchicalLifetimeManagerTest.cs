using System.Threading.Tasks;
using Fs.Container.Core;
using Fs.Container.Core.Lifetime;
using Fs.Container.Test.Core.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Core.Lifetime
{
    [TestClass]
    public class HierarchicalLifetimeManagerTest
    {
        private IFsContainer container;
        private IFsContainer child;
        private IFsContainer nestedChild;

        public HierarchicalLifetimeManagerTest()
        {
            container = new FsContainer();
            container.For<ILogger>().Use<Logger>(new HierarchicalLifetimeManager());
            child = container.CreateChildContainer();
            nestedChild = container.CreateChildContainer();
        }

        [TestMethod]
        public void TestParentResolveActsLikeContainerControlledLifetime()
        {
            var o1 = container.Resolve<ILogger>();
            var o2 = container.Resolve<ILogger>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task TestMultiThreadParentResolveActsLikeContainerControlledLifetime()
        {
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

            var o1 = instances[0];
            var o2 = instances[1];

            // Assert
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public void TestParentAndChildResolveDifferentInstances()
        {
            var o1 = container.Resolve<ILogger>();
            var o2 = child.Resolve<ILogger>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task TestMultiThreadParentAndChildResolveDifferentInstances()
        {
            // Act
            var instances = await Task.WhenAll(
                Task.Run(() => {
                    Task.Delay(10);
                    return container.Resolve<ILogger>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return child.Resolve<ILogger>();
                })
            );

            var o1 = instances[0];
            var o2 = instances[1];

            // Assert
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public void TestChildResolvesTheSameInstance()
        {
            // Act
            var o1 = child.Resolve<ILogger>();
            var o2 = child.Resolve<ILogger>();

            // Assert
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task TestMultiThreadChildResolvesTheSameInstance()
        {
            // Act
            var instances = await Task.WhenAll(
                Task.Run(() => {
                    Task.Delay(10);
                    return child.Resolve<ILogger>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return child.Resolve<ILogger>();
                })
            );

            var o1 = instances[0];
            var o2 = instances[1];

            // Assert
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public void TestSiblingContainersResolveDifferentInstances()
        {
            var o1 = child.Resolve<ILogger>();
            var o2 = nestedChild.Resolve<ILogger>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task TestMultiThreadSiblingContainersResolveDifferentInstances()
        {
            // Act
            var instances = await Task.WhenAll(
                Task.Run(() => {
                    Task.Delay(10);
                    return child.Resolve<ILogger>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return nestedChild.Resolve<ILogger>();
                })
            );

            var o1 = instances[0];
            var o2 = instances[1];

            // Assert
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public void DisposingOfChildContainerDisposesOnlyChildObject()
        {
            var o1 = container.Resolve<DisposableObject>();
            var o2 = child.Resolve<DisposableObject>();

            child.Dispose();
            Assert.IsFalse(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
        }

        [TestMethod]
        public void DisposingOfParentContainerDisposesChildAndParentObject()
        {
            var o1 = container.Resolve<DisposableObject>();
            var o2 = child.Resolve<DisposableObject>();

            container.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
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
