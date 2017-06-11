using System;
using Fs.Container.Core.Syntax;

namespace Fs.Container.Core
{
    public interface IFsContainer : IBindingRoot, IDisposable
    {
        T Resolve<T>();

        object Resolve(Type type);

        IFsContainer Parent { get; }

        IFsContainer CreateChildContainer();
    }
}