using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fs.Container.Lifetime;

namespace Fs.Container.Bindings
{
    public class BindingConfiguration : IBindingConfiguration
    {        
        public Type Concrete {get; set;}
        public ILifetimeManager Lifetime {get; set;}
        public IDictionary<string, object> Arguments { get; set; }
        public Func<IFsContainer, object> FactoryFunc { get; set; }
    }
}
