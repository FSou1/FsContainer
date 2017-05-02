using Fs.Container.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Bindings
{
    public interface IBinding : IBindingConfiguration
    {
        Type Service { get; }
        IBindingConfiguration BindingConfiguration { get; }
    }
}
