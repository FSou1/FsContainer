using System;
using System.Threading.Tasks;
using Fs.Container.Syntax;

namespace Fs.Container
{
    public interface IFsContainer : IBindingRoot, IDisposable
    {
        IFsContainer Parent { get; }
        IFsContainer CreateChildContainer();

        T Resolve<T>();
        object Resolve(Type type);

        Task<T> ResolveAsync<T>();
        Task<object> ResolveAsync(Type type);
    }
}