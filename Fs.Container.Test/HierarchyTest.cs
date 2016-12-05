using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Test
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
            Assert.AreEqual(child.Parent, container);
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
            Assert.AreNotEqual(child, child1);
        }
    }
}
