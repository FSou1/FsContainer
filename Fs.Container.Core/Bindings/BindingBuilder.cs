using System;
using Fs.Container.Core.Lifetime;
using Fs.Container.Core.Syntax;
using Fs.Container.Core.Utility;

namespace Fs.Container.Core.Bindings {
    public class BindingBuilder<T> : IBindingUseSyntax {
        private IBinding _binding { get; }

        public BindingBuilder(IBinding binding) {
            _binding = binding;
        }

        public IBindingWithSyntax<T> Use<T>() {
            return Use<T>(new TransientLifetimeManager());
        }

        public void Use(Func<IFsContainer, object> factoryFunc)
        {
            Use(factoryFunc, new TransientLifetimeManager());
        }

        public void Use(Func<IFsContainer, object> factoryFunc, ILifetimeManager lifetimeManager)
        {
            Guard.ArgumentNotNull(factoryFunc, nameof(factoryFunc));

            _binding.FactoryFunc = factoryFunc;
            _binding.Lifetime = lifetimeManager;
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