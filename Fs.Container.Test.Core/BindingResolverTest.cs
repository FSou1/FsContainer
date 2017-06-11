using System;
using System.Collections.Generic;
using Fs.Container.Core;
using Fs.Container.Core.Bindings;
using Fs.Container.Core.Resolve;
using Fs.Container.Test.Core.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test.Core
{
    [TestClass]
    public class BindingResolverTest
    {
        [TestMethod]
        [ExpectedException(typeof(CustomBindingBindingResolver.CustomBindingResolverException))]
        public void RegisteredServicesResolvedWithCustomBindingResolver()
        {
            // Arrange
            var container = new FsContainer
            {
                BindingResolver = new CustomBindingBindingResolver()
            };
            container.For<IValidator>().Use<Validator>();

            // Act
            container.Resolve<IValidator>();
        }
        
        internal class CustomBindingBindingResolver : IBindingResolver
        {
            internal class CustomBindingResolverException : Exception { }

            public object Resolve(IFsContainer container, IEnumerable<IBinding> bindings, Type service)
            {
                throw new CustomBindingResolverException();
            }
        }
    }
}