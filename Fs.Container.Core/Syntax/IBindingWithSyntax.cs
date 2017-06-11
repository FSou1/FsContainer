namespace Fs.Container.Core.Syntax
{
    public interface IBindingWithSyntax<T> 
    {
        IBindingWithSyntax<T> WithConstructorArgument(
            string argumentName, object argumentValue
        );
    }
}
