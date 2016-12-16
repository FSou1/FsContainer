using System;
using Fs.Container.Syntax;
using System.Collections;
using System.Collections.Generic;
using Fs.Container.Bindings;

namespace Fs.Container
{
    public class BindingConfigurationBuilder<T> : IBindingWithSyntax<T>
    {
        private IBindingConfiguration _bindingConfiguration;

        public BindingConfigurationBuilder(
            IBindingConfiguration bindingConfiguration)
        {
            this._bindingConfiguration = bindingConfiguration;
        }

        public IDictionary<string, object> Arguments { get; set; }

        public IBindingWithSyntax<T> WithConstructorArgument(
            string argumentName, object argumentValue
        )
        {
            if (_bindingConfiguration.Arguments == null)
                _bindingConfiguration.Arguments = new Dictionary<string, object>();

            _bindingConfiguration.Arguments[argumentName] = argumentValue;

            return this;
        }
    }
}
