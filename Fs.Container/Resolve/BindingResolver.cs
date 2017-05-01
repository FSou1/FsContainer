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
        public object Resolve(IEnumerable<IBinding> bindings, Type service)
        {
            Guard.ArgumentNotNull(bindings, nameof(bindings));

            var binding = bindings.FirstOrDefault(b => b.Service == service);

            var ctx = new BuildContext
            {
                Service = service,
                Concrete = binding?.Concrete,
                Arguments = binding?.Arguments,
                Bindings = bindings
            };

            var exist = binding?.Lifetime?.GetValue();
            if (exist != null)
            {
                return exist;
            }

            var instance = Build(ctx);

            if(binding?.Lifetime is PerResolveLifetimeManager)
            {
                binding.Lifetime = new PerResolveLifetimeManager(instance);
            }

            binding?.Lifetime?.SetValue(instance);

            return instance;
        }

        private object Build(BuildContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            if(context.Concrete != null)
            {
                return CreateInstance(context);
            }

            if(context.Service.GetConstructor(Type.EmptyTypes) == null)
            {
                return CreateInstance(context);
            }

            return Activator.CreateInstance(context.Service);
        }

        private object CreateInstance(BuildContext context)
        {
            Guard.ArgumentNotNull(context, nameof(context));

            var concrete = context.Concrete ?? context.Service;
            var constructorArguments = context.Arguments ?? new Dictionary<string, object>();

            var ctor = new ConstructorScorer(concrete, constructorArguments).GetConstructor();

            var parameters = ctor.GetParameters();
            var arguments = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var argument = constructorArguments.ContainsKey(parameter.Name)
                    ? constructorArguments[parameter.Name]
                    : Resolve(context.Bindings, parameter.ParameterType);

                arguments[i] = argument;
            }

            return ctor.Invoke(arguments);
        }
    }

    public class BuildContext
    {
        public Type Service { get; set; }

        public Type Concrete { get; set; }
        
        public IDictionary<string, object> Arguments { get; set; }

        public IEnumerable<IBinding> Bindings { get; set; }
    }
}