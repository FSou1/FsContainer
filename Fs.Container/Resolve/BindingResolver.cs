using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fs.Container.Bindings;
using Fs.Container.Lifetime;
using Fs.Container.Utility;

namespace Fs.Container.Resolve
{
    public class BindingResolver : IBindingResolver
    {
        public class ResolveContext
        {
            public IFsContainer Container { get; set; }
            public IEnumerable<IBinding> Bindings { get; set; }
        }

        public object Resolve(IFsContainer container, IEnumerable<IBinding> bindings, Type service)
        {
            Guard.ArgumentNotNull(bindings, nameof(bindings));

            foreach (var binding in bindings)
            {
                if (binding.Lifetime is PerResolveLifetimeManager)
                {
                    binding.Lifetime = new PerResolveLifetimeManager();
                }
            }

            var context = new ResolveContext
            {
                Container = container,
                Bindings = bindings
            };

            return Resolve(context, service);
        }

        private object Resolve(ResolveContext context, Type service)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            var binding = context.Bindings.FirstOrDefault(b => b.Service == service);
            if(binding == null)
            {
                binding = new Binding(service);
            }

            var exist = binding.Lifetime?.GetValue();
            if (exist != null)
            {
                return exist;
            }

            var instance = Build(context, binding);

            if(binding.Lifetime is PerResolveLifetimeManager)
            {
                binding.Lifetime = new PerResolveLifetimeManager(instance);
            }

            binding.Lifetime?.SetValue(instance);

            return instance;
        }

        private object Build(ResolveContext context, IBinding binding)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(binding, nameof(binding));

            if (binding.FactoryFunc != null)
            {
                return binding.FactoryFunc(context.Container);
            }

            if (binding.FactoryFuncAsync != null) {
                return binding.FactoryFuncAsync(context.Container).Result;
            }

            if(binding.Concrete != null)
            {
                return CreateInstance(context, binding);
            }

            if(binding.Service.GetConstructor(Type.EmptyTypes) == null)
            {
                return CreateInstance(context, binding);
            }

            return Activator.CreateInstance(binding.Service);
        }

        private object CreateInstance(ResolveContext context, IBinding binding)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            Guard.ArgumentNotNull(binding, nameof(binding));

            var concrete = binding.Concrete ?? binding.Service;
            var constructorArguments = binding.Arguments ?? new Dictionary<string, object>();

            var ctor = new ConstructorScorer(concrete, constructorArguments).GetConstructor();

            var parameters = ctor.GetParameters();
            var arguments = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var argument = constructorArguments.ContainsKey(parameter.Name)
                    ? constructorArguments[parameter.Name]
                    : Resolve(context, parameter.ParameterType);

                arguments[i] = argument;
            }

            return ctor.Invoke(arguments);
        }
    }
}