using System.Threading.Tasks;
using Fs.Container.Lifetime;
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
        public void TestContainerUseFactoryMethodAsyncToCreateTheObject()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .UseAsync(async ctx => await Repository.CreateInstanceAsync("sql_connection_string"));

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
        public void TestContainerUseTransientLifetimeManagerAsDefaultWithFactoryAsyncMethod()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .UseAsync(async ctx => await Repository.CreateInstanceAsync("sql_connection_string"));

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
        public void TestContainerUseExistingObjectFromLifetimeManagerWithFactoryMethodAsync()
        {
            // Arrange
            var container = new FsContainer();

            container
                .For<IRepository>()
                .UseAsync(async ctx => await Repository.CreateInstanceAsync("sql_connection_string"), new ContainerControlledLifetimeManager());

            // Act
            var first = container.Resolve<IRepository>();
            var second = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(first.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.ConnectionString, "sql_connection_string");
            Assert.AreSame(first, second);
        }
    }
}