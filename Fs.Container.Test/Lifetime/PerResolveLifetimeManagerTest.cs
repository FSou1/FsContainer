using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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
        public async Task TestPerResolveInstanceIsSame()
        {
            // Arrange
            var controller = await container.ResolveAsync<Controller>();

            // Assert
            Assert.IsNotNull(controller.Mapper);
            Assert.IsNotNull(controller.Service.Mapper);
            Assert.AreSame(controller.Mapper, controller.Service.Mapper);
        }

        [TestMethod]
        public async Task TestPerResolveInstancesAreNotSame()
        {
            // Arrange
            var firstController = await container.ResolveAsync<Controller>();
            var secondController = await container.ResolveAsync<Controller>();
            
            // Assert
            Assert.IsNotNull(firstController.Mapper);
            Assert.IsNotNull(secondController.Mapper);
            Assert.AreNotSame(firstController.Mapper, secondController.Mapper);
        }
    }
}