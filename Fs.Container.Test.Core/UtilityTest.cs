using System;
using Fs.Container.Core.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Core
{
    [TestClass]
    public class UtilityTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullThrowsArgumentNullException()
        {
            // Act
            object argument = null;

            // Arrange
            Guard.ArgumentNotNull(argument, nameof(argument));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ArgumentNotNullOrEmptyThrowsArgumentNullException()
        {
            // Act
            string argument = null;

            // Arrange
            Guard.ArgumentNotNullOrEmpty(argument, nameof(argument));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ArgumentNotNullOrEmptyThrowsArgumentException()
        {
            // Act
            string argument = string.Empty;

            // Arrange
            Guard.ArgumentNotNullOrEmpty(argument, nameof(argument));
        }
    }
}
