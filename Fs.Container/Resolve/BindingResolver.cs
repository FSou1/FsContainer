using System;
using System.Collections.Generic;
using System.Linq;
using Fs.Container.Bindings;
using Fs.Container.Utility;

namespace Fs.Container.Resolve
{
    public class BindingResolver : IBindingResolver
    {
        public object Resolve(IEnumerable<IBinding> bindings, Type service)
        {
            Guard.ArgumentNotNull(bindings, nameof(bindings));

            var binding = bindings.FirstOrDefault(b => b.Service == service);

            var instance = binding != null 
                ? CreateInstance(bindings, binding) 
                : CreateInstance(bindings, service);

            return instance;
        }

        public object CreateInstance(IEnumerable<IBinding> bindings, IBinding binding)
        {
            var concrete = binding.Concrete;
            var lifetimeManager = binding.Lifetime;
            var arguments = binding.Arguments
                ?? new Dictionary<string, object>();

            var exist = lifetimeManager.GetValue();

            if (exist == null)
            {
                exist = CreateInstance(bindings, concrete, arguments);
                lifetimeManager.SetValue(exist);
            }

            return exist;
        }

        private object CreateInstance(IEnumerable<IBinding> bindings, Type concrete)
        {
            if (concrete.GetConstructor(Type.EmptyTypes) != null)
            {
                return Activator.CreateInstance(concrete);
            }

            return CreateInstance(bindings, concrete, new Dictionary<string, object>());
        }

        private object CreateInstance(IEnumerable<IBinding> bindings, Type concrete, IDictionary<string, object> constructorArguments)
        {
            var ctor = new ConstructorScorer(concrete, constructorArguments).GetConstructor();

            var parameters = ctor.GetParameters();
            var arguments = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var argument = constructorArguments.ContainsKey(parameter.Name)
                    ? constructorArguments[parameter.Name]
                    : this.Resolve(bindings, parameter.ParameterType);

                arguments[i] = argument;
            }

            return ctor.Invoke(arguments);
        }
    }
}