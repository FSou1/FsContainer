using System;
using Fs.Container.Syntax;

namespace Fs.Container
{
    public interface IFsContainer : IBindingRoot, IDisposable
    {
        T Resolve<T>();

        object Resolve(Type type);

        IFsContainer Parent { get; }

        IFsContainer CreateChildContainer();
    }
}