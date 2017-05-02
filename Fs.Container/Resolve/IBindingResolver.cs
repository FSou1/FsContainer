using System;
using System.Collections.Generic;
using Fs.Container.Bindings;

namespace Fs.Container.Resolve
{
    public interface IBindingResolver
    {
        object Resolve(FsContainer container, IEnumerable<IBinding> bindings, Type service);
    }
}