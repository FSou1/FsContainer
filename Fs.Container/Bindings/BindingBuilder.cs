using Fs.Container.Lifetime;
using Fs.Container.Syntax;
using Fs.Container.Utility;
using System;
using System.Collections.Generic;

namespace Fs.Container.Bindings {
    public class BindingBuilder<T> : IBindingUseSyntax<T> {
        private IBinding _binding { get; set; }

        public BindingBuilder(IBinding binding) {
            _binding = binding;
        }

        public IBindingWithSyntax<T> Use<T>() {
            return Use<T>(new TransientLifetimeManager());
        }

        public IBindingWithSyntax<T> Use<T>(ILifetimeManager lifetime) {
            Guard.ArgumentNotNull(lifetime, nameof(lifetime));
            
            _binding.Concrete = typeof(T);
            _binding.Lifetime = lifetime;

            Guard.TypeIsAssignable(_binding.Service, _binding.Concrete);

            return new BindingConfigurationBuilder<T>(_binding.BindingConfiguration);
        }
    }
}