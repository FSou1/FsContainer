using Fs.Container.Bindings;
using Fs.Container.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Syntax
{
    public abstract class BindingRoot : IBindingRoot
    {
        public IBindingUseSyntax<T> For<T>()
        {
            var type = typeof(T);

            var binding = new Binding(type);
            this.AddBinding(binding);

            return new BindingBuilder<T>(binding);
        }

        protected IEnumerable<IBinding> GetBindings()
        {
            return this.Bindings;
        }

        protected IEnumerable<IBinding> GetBindings(Type service)
        {
            return this.Bindings.Where(t => t.Service == service);
        }

        private void AddBinding(IBinding binding)
        {
            Guard.ArgumentNotNull(binding, nameof(binding));
            Bindings.Add(binding);
        }

        private IList<IBinding> Bindings { get; set; } = new List<IBinding>();
    }
}
