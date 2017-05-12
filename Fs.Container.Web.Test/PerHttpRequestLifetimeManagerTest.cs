using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fs.Container.Web.Lifetime;
using System.Web;
using System.IO;
using Fs.Container.TestObjects;

namespace Fs.Container.Web.Test
{
    [TestClass]
    public class PerHttpRequestLifetimeManagerTest
    {
        private FsContainer container;

        public PerHttpRequestLifetimeManagerTest()
        {
            container = new FsContainer();

            container
                .For<IMapper>()
                .Use<Mapper>(new PerHttpRequestLifetimeManager());
        }

        [TestMethod]
        public async Task TestPerSingleHttpRequestInstancesAlwaysSame()
        {
            // Arrange
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://github.com/FSou1/FsContainer", ""),
                new HttpResponse(new StringWriter())
            );

            var controller = await container.ResolveAsync<Controller>();
            var firstMapper = await container.ResolveAsync<IMapper>();
            var secondMapper = await container.ResolveAsync<IMapper>();

            // Assert
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.Mapper);
            Assert.IsNotNull(firstMapper);
            Assert.IsNotNull(secondMapper);
            Assert.AreSame(controller.Mapper, firstMapper);
            Assert.AreSame(controller.Mapper, secondMapper);
        }

        [TestMethod]
        public async Task TestPerMultipleHttpRequestInstancesAlwaysDifferent()
        {
            // Arrange
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://github.com/FSou1/FsContainer", ""),
                new HttpResponse(new StringWriter())
            );
            var firstMapper = await container.ResolveAsync<IMapper>();

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://github.com/FSou1", ""),
                new HttpResponse(new StringWriter())
            );
            var secondMapper = await container.ResolveAsync<IMapper>();

            // Assert
            Assert.IsNotNull(firstMapper);
            Assert.IsNotNull(secondMapper);
            Assert.AreNotSame(firstMapper, secondMapper);
        }
    }
}
