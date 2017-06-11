using System;
using Fs.Container.Core.Lifetime;

namespace Fs.Container.Core.Syntax
{
    public interface IBindingUseSyntax
    {
        IBindingWithSyntax<T> Use<T>();
        IBindingWithSyntax<T> Use<T>(ILifetimeManager lifetimeManager);

        void Use(Func<IFsContainer, object> factoryFunc);
        void Use(Func<IFsContainer, object> factoryFunc, ILifetimeManager lifetimeManager);
    }
}
