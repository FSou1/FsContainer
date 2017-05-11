using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fs.Container.Bindings;

namespace Fs.Container.Resolve
{
    public interface IBindingResolver
    {
        object Resolve(
            IFsContainer container, IEnumerable<IBinding> bindings, Type service);

        Task<object> ResolveAsync(
            IFsContainer container, IEnumerable<IBinding> bindings, Type service);
    }
}