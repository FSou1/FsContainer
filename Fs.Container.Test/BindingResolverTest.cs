using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fs.Container.Bindings;
using Fs.Container.Resolve;
using Fs.Container.TestObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fs.Container.Test
{
    [TestClass]
    public class BindingResolverTest
    {
        [TestMethod]
        [ExpectedException(typeof(CustomBindingBindingResolver.CustomBindingResolverException))]
        public async Task TestRegisteredServicesResolvedWithCustomBindingResolver()
        {
            // Arrange
            var container = new FsContainer
            {
                BindingResolver = new CustomBindingBindingResolver()
            };
            container.For<IValidator>().Use<Validator>();

            // Act
            await container.ResolveAsync<IValidator>();
        }

        internal class CustomBindingBindingResolver : IBindingResolver
        {
            internal class CustomBindingResolverException : Exception { }
            
            public Task<object> ResolveAsync(IFsContainer container, IEnumerable<IBinding> bindings, Type service) {
                throw new CustomBindingResolverException();
            }
        }
    }
}