using Fs.Container.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Syntax
{
    public interface IBindingUseSyntax<T>
    {
        IBindingWithSyntax<T> Use<T>();
        IBindingWithSyntax<T> Use<T>(ILifetimeManager lifetimeManager);

        void Use(Func<FsContainer, object> factoryFunc);
        void Use(Func<FsContainer, object> factoryFunc, ILifetimeManager lifetimeManager);
    }
}
