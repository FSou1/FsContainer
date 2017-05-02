using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Lifetime
{
    [TestClass]
    public class HierarchicalLifetimeManagerTest
    {
        private IFsContainer child1;
        private IFsContainer child2;
        private IFsContainer parent;
        
        public HierarchicalLifetimeManagerTest()
        {
            parent = new FsContainer();
            parent.For<ILogger>().Use<Logger>(new HierarchicalLifetimeManager());
            parent.For<DisposableObject>()
                .Use<DisposableObject>(new HierarchicalLifetimeManager());
            child1 = parent.CreateChildContainer();
            child2 = parent.CreateChildContainer();
        }

        [TestMethod]
        public void ParentResolveActsLikeContainerControlledLifetime()
        {
            var o1 = parent.Resolve<ILogger>();
            var o2 = parent.Resolve<ILogger>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public void ParentAndChildResolveDifferentInstances()
        {
            var o1 = parent.Resolve<ILogger>();
            var o2 = child1.Resolve<ILogger>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public void ChildResolvesTheSameInstance()
        {
            var o1 = child1.Resolve<ILogger>();
            var o2 = child1.Resolve<ILogger>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public void SiblingContainersResolveDifferentInstances()
        {
            var o1 = child1.Resolve<ILogger>();
            var o2 = child2.Resolve<ILogger>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public void DisposingOfChildContainerDisposesOnlyChildObject()
        {
            var o1 = parent.Resolve<DisposableObject>();
            var o2 = child1.Resolve<DisposableObject>();

            child1.Dispose();
            Assert.IsFalse(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
        }

        [TestMethod]
        public void DisposingOfParentContainerDisposesChildAndParentObject()
        {
            var o1 = parent.Resolve<DisposableObject>();
            var o2 = child1.Resolve<DisposableObject>();

            parent.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
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
