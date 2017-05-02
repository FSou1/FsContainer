using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fs.Container.Bindings;

namespace Fs.Container.Syntax
{
    public interface IBindingRoot
    {
        IBindingUseSyntax For<T>();
    }
}
