using Fs.Ninject.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Ninject.Test
{
    [TestClass]
    public class HierarchicalLifetimeManagerTest
    {
        private StandardKernel child1;
        private StandardKernel child2;
        private StandardKernel parent;

        public HierarchicalLifetimeManagerTest()
        {
            parent = new StandardKernel();
            parent.Bind<ILogger>().To<Logger>();
            parent.Bind<DisposableObject>().To<DisposableObject>();
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
    }
}
