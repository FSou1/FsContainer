﻿using Fs.Container.Lifetime;
using Fs.Container.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Test {
    [TestClass]
    public class ContainerControlledLifetimeManagerTest {        
        [TestMethod]
        public void TestContainerControlledLifetimeInstanceAlwaysSame() {
            // Act
            var container = new FsContainer();
            container.For<ILogger>().Use<Logger>(new ContainerControlledLifetimeManager());

            // Arrange
            var logger = container.Resolve<ILogger>();
            var logger1 = container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsNotNull(logger1);
            Assert.AreSame(logger, logger1);
        }
    }
}