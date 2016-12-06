using Fs.Container.Lifetime;
using Fs.Container.Utility;
using System;
using System.Collections.Generic;

namespace Fs.Container {
    public class BindingBuilder {
        internal Type _service;
        internal Type _concrete;
        internal ILifetimeManager _lifetime;
        internal Dictionary<string, object> _arguments;

        public BindingBuilder(Type service) {
            _service = service;
        }

        public BindingBuilder Use<T>() {
            return Use<T>(new TransientLifetimeManager());
        }

        public BindingBuilder Use<T>(ILifetimeManager lifetime) {
            Guard.ArgumentNotNull(lifetime, nameof(lifetime));
            
            _concrete = typeof(T);
            _lifetime = lifetime;

            Guard.TypeIsAssignable(_service, _concrete);

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