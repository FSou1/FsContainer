using System.Threading.Tasks;
using Fs.Container.Lifetime;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.BindingBuilder
{
    [TestClass]
    public class BindingBuilderTest {
        private readonly IFsContainer container;

        private async Task<object> ResolveRepositoryAsync(IFsContainer ctx) {
            return await Repository.CreateInstanceAsync(await ctx.ResolveAsync<IFakeDbConnection>());
        }

        public BindingBuilderTest() {
            container = new FsContainer();

            container
                .For<IFakeDbConnection>()
                .Use(ctx => {
                    var conn = new FakeSqlConnection("sql_connection_string");
                    conn.EnsureOpen();
                    return conn;
                });

            container.For<IFakeDbConnection>()
                .UseAsync(async ctx => {
                    var conn = new FakeSqlConnection("sql_connection_string");
                    await conn.EnsureOpenAsync();
                    return conn;
                });
        }

        [TestMethod]
        public void TestContainerUseFactoryMethodToCreateTheObject()
        {
            // Arrange
            container
                .For<IRepository>()
                .Use(ctx => new Repository(ctx.Resolve<IFakeDbConnection>()));

            // Act
            var repository = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(repository.Connection.IsOpen, true);
            Assert.AreEqual(repository.Connection.ConnectionString, "sql_connection_string");
        }

        [TestMethod]
        public async void TestContainerUseFactoryMethodAsyncToCreateTheObject()
        {
            // Arrange
            container
                .For<IRepository>()
                .UseAsync(async ctx => await ResolveRepositoryAsync(ctx));

            // Act
            var repository = await container.ResolveAsync<IRepository>();

            // Arrange
            Assert.AreEqual(repository.Connection.IsOpen, true);
            Assert.AreEqual(repository.Connection.ConnectionString, "sql_connection_string");
        }

        [TestMethod]
        public void TestContainerUseTransientLifetimeManagerAsDefaultWithFactoryMethod()
        {
            // Arrange
            container
                .For<IRepository>()
                .Use(ctx => new Repository(ctx.Resolve<IFakeDbConnection>()));

            // Act
            var first = container.Resolve<IRepository>();
            var second = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(first.Connection.IsOpen, true);
            Assert.AreEqual(first.Connection.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.Connection.IsOpen, true);
            Assert.AreEqual(second.Connection.ConnectionString, "sql_connection_string");
            Assert.AreNotSame(first, second);
        }

        [TestMethod]
        public async void TestContainerUseTransientLifetimeManagerAsDefaultWithFactoryAsyncMethod()
        {
            // Arrange
            container
                .For<IRepository>()
                .UseAsync(async ctx => await ResolveRepositoryAsync(ctx));

            // Act
            var first = await container.ResolveAsync<IRepository>();
            var second = await container.ResolveAsync<IRepository>();

            // Arrange
            Assert.AreEqual(first.Connection.IsOpen, true);
            Assert.AreEqual(first.Connection.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.Connection.IsOpen, true);
            Assert.AreEqual(second.Connection.ConnectionString, "sql_connection_string");
            Assert.AreNotSame(first, second);
        }

        [TestMethod]
        public void TestContainerUseExistingObjectFromLifetimeManagerWithFactoryMethod()
        {
            // Arrange
            container
                .For<IRepository>()
                .Use(ctx => new Repository(ctx.Resolve<IFakeDbConnection>()), new ContainerControlledLifetimeManager());

            // Act
            var first = container.Resolve<IRepository>();
            var second = container.Resolve<IRepository>();

            // Arrange
            Assert.AreEqual(first.Connection.IsOpen, true);
            Assert.AreEqual(first.Connection.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.Connection.IsOpen, true);
            Assert.AreEqual(second.Connection.ConnectionString, "sql_connection_string");
            Assert.AreSame(first, second);
        }

        [TestMethod]
        public async void TestContainerUseExistingObjectFromLifetimeManagerWithFactoryMethodAsync()
        {
            // Arrange
            container
                .For<IRepository>()
                .UseAsync(async ctx => await ResolveRepositoryAsync(ctx), new ContainerControlledLifetimeManager());

            // Act
            var first = await container.ResolveAsync<IRepository>();
            var second = await container.ResolveAsync<IRepository>();

            // Arrange
            Assert.AreEqual(first.Connection.IsOpen, true);
            Assert.AreEqual(first.Connection.ConnectionString, "sql_connection_string");
            Assert.AreEqual(second.Connection.IsOpen, true);
            Assert.AreEqual(second.Connection.ConnectionString, "sql_connection_string");
            Assert.AreSame(first, second);
        }
    }
}