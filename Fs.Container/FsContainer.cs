using Fs.Container.Bindings;
using Fs.Container.Dispose;
using Fs.Container.Lifetime;
using Fs.Container.Syntax;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container {
    public class FsContainer : BindingRoot, IDisposable {
        private readonly FsContainer parent;

        private DisposeManager disposeManager;

        /// <summary>
        /// Create a default <see cref="FsContainer"/>
        /// </summary>
        public FsContainer() 
            : this(null) { }

        private FsContainer(FsContainer parent)
        {
            this.parent = parent;
            if(parent != null)
            {
                parent.disposeManager.Add(this);
            }

            this.disposeManager = new DisposeManager();
        }
        
        #region Resolve
        public T Resolve<T>() {
            return (T)Resolve(typeof (T));
        }

        public object Resolve(Type type) {
            var builder = this.GetBindings(type).FirstOrDefault();
            var instance = builder != null ? CreateInstance(builder) : CreateInstance(type);

            if (!disposeManager.Contains(instance))
            {
                disposeManager.Add(instance);
            }
            
            return instance;
        }
        #endregion

        #region Child container
        public FsContainer Parent {
            get { return parent; }
        }

        public FsContainer CreateChildContainer()
        {
            var child = new FsContainer(this);
            var bindings = this.GetBindings().ToList();            
            foreach(var binding in bindings)
            {
                child.AddBinding(CloneBinding(binding));
            }
            return child;
        }
        #endregion

        public object CreateInstance(IBinding binding) {
            var concrete = binding.Concrete;
            var lifetimeManager = binding.Lifetime;
            var arguments = binding.Arguments 
                ?? new Dictionary<string, object>();

            var exist = lifetimeManager.GetValue();

            if (exist == null) {
                exist = CreateInstance(concrete, arguments);
                lifetimeManager.SetValue(exist);
            }

            return exist;
        }

        public object CreateInstance(Type concrete) {
            if (concrete.GetConstructor(Type.EmptyTypes) != null) {
                return Activator.CreateInstance(concrete);
            }

            return CreateInstance(concrete, new Dictionary<string, object>());
        }

        public object CreateInstance(Type concrete, IDictionary<string, object> constructorArguments) {
            var ctor = new ConstructorScorer(concrete, constructorArguments).GetConstructor();

            var parameters = ctor.GetParameters();
            var arguments = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; i++) {
                var parameter = parameters[i];
                var argument = constructorArguments.ContainsKey(parameter.Name)
                    ? constructorArguments[parameter.Name]
                    : this.Resolve(parameter.ParameterType);

                arguments[i] = argument;
            }

            return ctor.Invoke(arguments);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Standard Dispose pattern implementation
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {                
                if(disposeManager != null)
                {
                    disposeManager.Dispose();
                    disposeManager = null;

                    if(parent?.disposeManager != null)
                    {
                        parent.disposeManager.Remove(this);
                    }
                }
            }
        }
    }
}
