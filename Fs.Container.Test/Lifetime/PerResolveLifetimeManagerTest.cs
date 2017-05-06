﻿using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Lifetime
{
    [TestClass]
    public class PerResolveLifetimeManagerTest
    {
        private FsContainer container;

        public PerResolveLifetimeManagerTest()
        {
            container = new FsContainer();

            container.For<IMapper>().Use<Mapper>(new PerResolveLifetimeManager());
        }

        [TestMethod]
        public void TestPerResolveInstanceIsSame()
        {
            // Arrange
            var controller = container.Resolve<Controller>();

            // Assert
            Assert.IsNotNull(controller.Mapper);
            Assert.IsNotNull(controller.Service.Mapper);
            Assert.AreSame(controller.Mapper, controller.Service.Mapper);
        }

        [TestMethod]
        public void TestPerResolveInstancesAreNotSame()
        {
            // Arrange
            var firstController = container.Resolve<Controller>();
            var secondController = container.Resolve<Controller>();
            
            // Assert
            Assert.IsNotNull(firstController.Mapper);
            Assert.IsNotNull(secondController.Mapper);
            Assert.AreNotSame(firstController.Mapper, secondController.Mapper);
        }
    }
}