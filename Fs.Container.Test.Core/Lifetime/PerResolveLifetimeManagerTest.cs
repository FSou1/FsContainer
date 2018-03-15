using System.Threading.Tasks;
using Fs.Container.Core;
using Fs.Container.Core.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Core.Lifetime
{
    [TestClass]
    public class PerResolveLifetimeManagerTest
    {
        private FsContainer container;

        public PerResolveLifetimeManagerTest()
        {
            container = new FsContainer();

            container
                .For<IMapper>()
                .Use<Mapper>(new PerResolveLifetimeManager());
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

        [TestMethod]
        public async Task TestMultiThreadPerResolveInstancesAreNotSame()
        {
            // Arrange
            var instances = await Task.WhenAll(
                Task.Run(() => {
                    Task.Delay(10);
                    return container.Resolve<Controller>();
                }),
                Task.Run(() => {
                    Task.Delay(10);
                    return container.Resolve<Controller>();
                })
            );

            var firstController = instances[0];
            var secondController = instances[1];

            // Assert
            Assert.IsNotNull(firstController.Mapper);
            Assert.IsNotNull(secondController.Mapper);
            Assert.AreNotSame(firstController.Mapper, secondController.Mapper);
        }
    }
}