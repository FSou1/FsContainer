﻿using Fs.Container.Lifetime;
using Fs.Container.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Test
{
    [TestClass]
    public class HierarchicalLifetimeManagerTest
    {
        private FsContainer child1;
        private FsContainer child2;
        private FsContainer parent;

        public HierarchicalLifetimeManagerTest()
        {
            parent = new FsContainer();
            child1 = parent.CreateChildContainer();
            child2 = parent.CreateChildContainer();
            parent.For<ILogger>().Use<Logger>(new HierarchicalLifetimeManager());
            parent.For<DisposableObject>()
                .Use<DisposableObject>(new HierarchicalLifetimeManager());
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
            var o2 = child1.Resolve<ILogger>();
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
    }
}