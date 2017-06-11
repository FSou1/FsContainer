using System;
using System.Collections.Generic;
using System.Linq;
using Fs.Container.Core.Bindings;
using Fs.Container.Core.Lifetime;
using Fs.Container.Core.Utility;

namespace Fs.Container.Core.Syntax
{
    public abstract class BindingRoot : IBindingRoot
    {
        public IBindingUseSyntax For<T>()
        {
            var type = typeof(T);
            var binding = new Binding(type);

            this.AddBinding(binding);

            return new BindingBuilder<T>(binding);
        }

        public void AddBinding(IBinding binding)
        {
            Guard.ArgumentNotNull(binding, nameof(binding));

            Bindings.Add(binding);
        }

        public IBinding CloneBinding(IBinding binding)
        {
            Guard.ArgumentNotNull(binding, nameof(binding));

            var clone = new Binding(
                binding.Service, 
                binding.Concrete, 
                binding.Arguments, 
                binding.Lifetime
            );

            if (clone.Lifetime is HierarchicalLifetimeManager)
            {
                clone.Lifetime = new HierarchicalLifetimeManager();
            }

            return clone;
        }

        protected IEnumerable<IBinding> GetBindings()
        {
            return this.Bindings;
        }

        protected IEnumerable<IBinding> GetBindings(Type service)
        {
            return this.Bindings.Where(t => t.Service == service);
        }

        private IList<IBinding> Bindings { get; } = new List<IBinding>();
    }
}
