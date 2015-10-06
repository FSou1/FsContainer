using System;
using System.Collections.Generic;

namespace Fs.Container {
    public class BindingBuilder {
        internal Type _service;
        internal Type _concrete;
        internal Dictionary<string, object> _arguments;

        public BindingBuilder(Type service) {
            _service = service;
        }

        public BindingBuilder Use<T>() {
            _concrete = typeof(T);
            return this;
        }

        public BindingBuilder WithConstructorArgument(string argumentName, object argumentValue) {
            if(_arguments == null)
                _arguments = new Dictionary<string, object>();

            _arguments[argumentName] = argumentValue;
            return this;
        }
    }
}