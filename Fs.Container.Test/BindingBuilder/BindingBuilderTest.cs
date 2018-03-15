using System.Threading.Tasks;
using Fs.Container.Core;
using Fs.Container.Core.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.BindingBuilder
{
    [TestClass]
    public class BindingBuilderTest
    {
        [TestMethod]
        public void TestContainerUseFactoryMethodToCreateTheObject()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .Use(ctx => new Repository("sql_connection_string"));

            // Act
            var repository = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(repository.ConnectionString, "sql_connection_string");
        }

        [TestMethod]
        public void TestContainerUseTransientLifetimeManagerAsDefaultWithFactoryMethod()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .Use(ctx => new Repository("sql_connection_string"));

            // Act
            var first = container.Resolve<IRepository>();
            var second = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(first.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.ConnectionString, "sql_connection_string");
            Assert.AreNotSame(first, second);
        }

        [TestMethod]
        public void TestContainerUseExistingObjectFromLifetimeManagerWithFactoryMethod()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .Use(ctx => new Repository("sql_connection_string"), new ContainerControlledLifetimeManager());

            // Act
            var first = container.Resolve<IRepository>();
            var second = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(first.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.ConnectionString, "sql_connection_string");
            Assert.AreSame(first, second);
        }

        [TestMethod]
        public async Task TestMultiThreadContainerUseExistingObjectFromLifetimeManagerWithFactoryMethodAsync()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .Use(ctx => new Repository("sql_connection_string"), new ContainerControlledLifetimeManager());

            // Act
            var instances = await Task.WhenAll(
                Task.Run(() =>
                {
                    Task.Delay(10);
                    return container.Resolve<IRepository>();
                }),
                Task.Run(() =>
                {
                    Task.Delay(10);
                    return container.Resolve<IRepository>();
                })
            );

            var first = instances[0];
            var second = instances[1];

            // Arrange
            Assert.AreEqual(first.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.ConnectionString, "sql_connection_string");
            Assert.AreSame(first, second);
        }
    }
}