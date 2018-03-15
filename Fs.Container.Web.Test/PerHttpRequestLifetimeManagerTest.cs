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
using Fs.Container.Core;

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
        public void TestPerSingleHttpRequestInstancesAlwaysSame()
        {
            // Arrange
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://github.com/FSou1/FsContainer", ""),
                new HttpResponse(new StringWriter())
            );

            var controller = container.Resolve<Controller>();
            var firstMapper = container.Resolve<IMapper>();
            var secondMapper = container.Resolve<IMapper>();

            // Assert
            Assert.IsNotNull(controller);
            Assert.IsNotNull(controller.Mapper);
            Assert.IsNotNull(firstMapper);
            Assert.IsNotNull(secondMapper);
            Assert.AreSame(controller.Mapper, firstMapper);
            Assert.AreSame(controller.Mapper, secondMapper);
        }

        [TestMethod]
        public void TestPerMultipleHttpRequestInstancesAlwaysDifferent()
        {
            // Arrange
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://github.com/FSou1/FsContainer", ""),
                new HttpResponse(new StringWriter())
            );
            var firstMapper = container.Resolve<IMapper>();

            HttpContext.Current = new HttpContext(
                new HttpRequest("", "https://github.com/FSou1", ""),
                new HttpResponse(new StringWriter())
            );
            var secondMapper = container.Resolve<IMapper>();

            // Assert
            Assert.IsNotNull(firstMapper);
            Assert.IsNotNull(secondMapper);
            Assert.AreNotSame(firstMapper, secondMapper);
        }
    }
}
