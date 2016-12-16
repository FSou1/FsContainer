using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Syntax
{
    public interface IBindingWithSyntax<T> 
    {
        IBindingWithSyntax<T> WithConstructorArgument(
            string argumentName, object argumentValue
        );
    }
}
