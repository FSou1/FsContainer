using Fs.Container.Lifetime;
using Fs.Container.Test.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test
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
    }
}