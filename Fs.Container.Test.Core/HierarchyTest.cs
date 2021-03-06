﻿using Fs.Container.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Core
{
    [TestClass]
    public class HierarchyTest
    {
        [TestMethod]
        public void TestContainerCouldCreateChild()
        {
            // Act
            var container = new FsContainer();

            // Arrange
            var child = container.CreateChildContainer();

            // Assert
            Assert.IsNotNull(child);
            Assert.IsInstanceOfType(child, typeof(FsContainer));
            Assert.AreSame(child.Parent, container);
        }

        [TestMethod]
        public void TestContainerChildrenAreNotTheSame()
        {
            // Act
            var container = new FsContainer();

            // Arrange
            var child = container.CreateChildContainer();
            var child1 = container.CreateChildContainer();

            // Assert
            Assert.IsNotNull(child);
            Assert.IsNotNull(child1);
            Assert.IsInstanceOfType(child, typeof(FsContainer));
            Assert.IsInstanceOfType(child1, typeof(FsContainer));
            Assert.AreNotSame(child, child1);
        }
    }
}
