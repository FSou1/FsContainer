using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fs.Container.Core.Bindings;
using Fs.Container.Core.Lifetime;
using Fs.Container.Core.Utility;

namespace Fs.Container.Core.Resolve
{
    public class BindingResolver : IBindingResolver
    {
        private readonly object _locker = new object();

        private readonly IDictionary<Type, Tuple<ConstructorInfo, ParameterInfo[]>> _ctorCache = 
            new ConcurrentDictionary<Type, Tuple<ConstructorInfo, ParameterInfo[]>>();

        public object Resolve(IFsContainer container, IEnumerable<IBinding> bindings, Type service)
        {
            lock (_locker) {
                Guard.ArgumentNotNull(bindings, nameof(bindings));

                foreach (var binding in bindings) {
                    if (binding.Lifetime is PerResolveLifetimeManager) {
                        binding.Lifetime = new PerResolveLifetimeManager();
                    }
                }

                var context = new ResolveContext {
                    Container = container,
                    Bindings = bindings
                };

                return Resolve(context, service);
            }
        }

        private object Resolve(ResolveContext context, Type service)
        {
            Guard.ArgumentNotNull(context, nameof(context));
            
            var binding = context.Bindings.FirstOrDefault(b => b.Service == service);
            if (binding == null)
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
            if (binding.FactoryFunc != null)
            {
                return binding.FactoryFunc(context.Container);
            }

            if(binding.Concrete != null)
            {
                return CreateInstance(context, binding);
            }

            if(binding.Service.GetTypeInfo().GetConstructor(Type.EmptyTypes) == null)
            {
                return CreateInstance(context, binding);
            }

            return Activator.CreateInstance(binding.Service);
        }

        private object CreateInstance(ResolveContext context, IBinding binding)
        {
            var concrete = binding.Concrete ?? binding.Service;
            var constructorArguments = binding.Arguments ?? new Dictionary<string, object>();

            if (!_ctorCache.ContainsKey(concrete))
            {
                var constructor = new ConstructorScorer(concrete, constructorArguments).GetConstructor();
                _ctorCache[concrete] = Tuple.Create(constructor, constructor.GetParameters());
            }

            var entry = _ctorCache[concrete];
            var ctor = entry.Item1;
            var parameters = entry.Item2;
            var arguments = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (!constructorArguments.TryGetValue(parameter.Name, out arguments[i]))
                {
                    arguments[i] = Resolve(context, parameter.ParameterType);
                }
            }

            return ctor.Invoke(arguments);
        }

        internal class ResolveContext
        {
            internal IFsContainer Container { get; set; }
            internal IEnumerable<IBinding> Bindings { get; set; }
        }
    }
}