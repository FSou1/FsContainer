using Fs.Container.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Bindings
{
    public interface IBindingConfiguration
    {
        Type Concrete { get; set; }
        ILifetimeManager Lifetime { get; set; }
        IDictionary<string, object> Arguments { get; set; }
    }
}
