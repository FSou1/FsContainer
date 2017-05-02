using Fs.Container.Lifetime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Syntax
{
    public interface IBindingUseSyntax
    {
        IBindingWithSyntax<T> Use<T>();
        IBindingWithSyntax<T> Use<T>(ILifetimeManager lifetimeManager);

        void Use(Func<IFsContainer, object> factoryFunc);
        void Use(Func<IFsContainer, object> factoryFunc, ILifetimeManager lifetimeManager);
    }
}
