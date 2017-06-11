using System;
using System.Collections.Generic;
using Fs.Container.Core.Lifetime;

namespace Fs.Container.Core.Bindings
{
    public interface IBindingConfiguration
    {
        Type Concrete { get; set; }
        ILifetimeManager Lifetime { get; set; }
        IDictionary<string, object> Arguments { get; set; }
        Func<IFsContainer, object> FactoryFunc { get; set; }
    }
}
