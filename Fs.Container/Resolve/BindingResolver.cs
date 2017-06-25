using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Fs.Container.Bindings;
using Fs.Container.Lifetime;
using Fs.Container.Utility;

namespace Fs.Container.Resolve
{
    public class BindingResolver : IBindingResolver
    {
        private readonly object _locker = new object();

        private readonly ConcurrentDictionary<Type, Tuple<ConstructorInfo, ParameterInfo[]>> _ctorCache =
            new ConcurrentDictionary<Type, Tuple<ConstructorInfo, ParameterInfo[]>>();

        private readonly ConcurrentDictionary<Type, Func<object[], object>> _activatorCache =
            new ConcurrentDictionary<Type, Func<object[], object>>();

        public object Resolve(IFsContainer container, IEnumerable<IBinding> bindings, Type service)
        {
            lock (_locker)
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

            if (binding.Lifetime is PerResolveLifetimeManager)
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

            if (binding.Concrete != null)
            {
                return CreateInstance(context, binding);
            }

            if (binding.Service.GetConstructor(Type.EmptyTypes) == null)
            {
                return CreateInstance(context, binding);
            }

            return Activator.CreateInstance(binding.Service);
        }

        private object CreateInstance(ResolveContext context, IBinding binding)
        {
            var concrete = binding.Concrete ?? binding.Service;
            var constructorArguments = binding.Arguments ?? new Dictionary<string, object>();

            var ctorCacheEntry = _ctorCache.GetOrAdd(concrete, x => GetCtor(concrete, constructorArguments));

            var ctor = ctorCacheEntry.Item1;
            var parameters = ctorCacheEntry.Item2;

            var activator = _activatorCache.GetOrAdd(concrete, x => GetActivator(ctor, parameters));

            var arguments = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];

                if (!constructorArguments.TryGetValue(parameter.Name, out arguments[i]))
                {
                    arguments[i] = Resolve(context, parameter.ParameterType);
                }
            }

            return activator.Invoke(arguments);
        }

        private Tuple<ConstructorInfo, ParameterInfo[]> GetCtor(Type concrete, IDictionary<string, object> arguments)
        {
            var constructor = new ConstructorScorer(concrete, arguments).GetConstructor();
            return Tuple.Create(constructor, constructor.GetParameters());
        }

        private Func<object[], object> GetActivator(ConstructorInfo ctor, ParameterInfo[] parameters)
        {
            var p = Expression.Parameter(typeof(object[]), "args");
            var args = new Expression[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var a = Expression.ArrayAccess(p, Expression.Constant(i));
                args[i] = Expression.Convert(a, parameters[i].ParameterType);
            }

            var b = Expression.New(ctor, args);
            var l = Expression.Lambda<Func<object[], object>>(b, p);

            return l.Compile();
        }

        internal class ResolveContext
        {
            internal IFsContainer Container { get; set; }
            internal IEnumerable<IBinding> Bindings { get; set; }
        }
    }
}