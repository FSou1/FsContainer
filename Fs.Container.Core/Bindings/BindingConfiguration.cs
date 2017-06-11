using System;
using System.Collections.Generic;
using Fs.Container.Core.Lifetime;

namespace Fs.Container.Core.Bindings
{
    public class BindingConfiguration : IBindingConfiguration
    {        
        public Type Concrete {get; set;}
        public ILifetimeManager Lifetime {get; set;}
        public IDictionary<string, object> Arguments { get; set; }
        public Func<IFsContainer, object> FactoryFunc { get; set; }
    }
}
