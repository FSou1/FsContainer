using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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
        public async Task ParentResolveActsLikeContainerControlledLifetime()
        {
            var o1 = await parent.ResolveAsync<ILogger>();
            var o2 = await parent.ResolveAsync<ILogger>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task ParentAndChildResolveDifferentInstances()
        {
            var o1 = await parent.ResolveAsync<ILogger>();
            var o2 = await child1.ResolveAsync<ILogger>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task ChildResolvesTheSameInstance()
        {
            var o1 = await child1.ResolveAsync<ILogger>();
            var o2 = await child1.ResolveAsync<ILogger>();
            Assert.AreSame(o1, o2);
        }

        [TestMethod]
        public async Task SiblingContainersResolveDifferentInstances()
        {
            var o1 = await child1.ResolveAsync<ILogger>();
            var o2 = await child2.ResolveAsync<ILogger>();
            Assert.AreNotSame(o1, o2);
        }

        [TestMethod]
        public async Task DisposingOfChildContainerDisposesOnlyChildObject()
        {
            var o1 = await parent.ResolveAsync<DisposableObject>();
            var o2 = await child1.ResolveAsync<DisposableObject>();

            child1.Dispose();
            Assert.IsFalse(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
        }

        [TestMethod]
        public async Task DisposingOfParentContainerDisposesChildAndParentObject()
        {
            var o1 = await parent.ResolveAsync<DisposableObject>();
            var o2 = await child1.ResolveAsync<DisposableObject>();

            parent.Dispose();
            Assert.IsTrue(o1.WasDisposed);
            Assert.IsTrue(o2.WasDisposed);
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
