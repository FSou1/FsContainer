using System;
using System.Collections.Generic;
using Fs.Container.Core.Bindings;

namespace Fs.Container.Core.Resolve
{
    public interface IBindingResolver
    {
        object Resolve(IFsContainer container, IEnumerable<IBinding> bindings, Type service);
    }
}