using System;
using System.Threading.Tasks;
using Fs.Container.Lifetime;
using Fs.Container.Syntax;
using Fs.Container.Utility;

namespace Fs.Container.Bindings {
    public class BindingBuilder<T> : IBindingUseSyntax {
        private IBinding _binding { get; }

        public BindingBuilder(IBinding binding) {
            _binding = binding;
        }

        public IBindingWithSyntax<T> Use<T>() {
            return Use<T>(new TransientLifetimeManager());
        }

        public void UseAsync(Func<IFsContainer, Task<object>> factoryFuncAsync) {
            Guard.ArgumentNotNull(factoryFuncAsync, nameof(factoryFuncAsync));

            _binding.FactoryFuncAsync = factoryFuncAsync;
        }

        public void UseAsync(Func<IFsContainer, Task<object>> factoryFuncAsync, ILifetimeManager lifetimeManager) {
            Guard.ArgumentNotNull(factoryFuncAsync, nameof(factoryFuncAsync));

            _binding.FactoryFuncAsync = factoryFuncAsync;
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