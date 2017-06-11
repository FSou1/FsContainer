using System;

namespace Fs.Container.Core.Bindings
{
    public interface IBinding : IBindingConfiguration
    {
        Type Service { get; }
        IBindingConfiguration BindingConfiguration { get; }
    }
}
