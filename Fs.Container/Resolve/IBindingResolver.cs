using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fs.Container.Bindings;

namespace Fs.Container.Resolve
{
    public interface IBindingResolver
    {
        Task<object> ResolveAsync(
            IFsContainer container, IEnumerable<IBinding> bindings, Type service);
    }
}