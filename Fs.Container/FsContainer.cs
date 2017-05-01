using Fs.Container.Bindings;
using Fs.Container.Dispose;
using Fs.Container.Lifetime;
using Fs.Container.Syntax;
using System;
using System.Linq;
using Fs.Container.Resolve;

namespace Fs.Container {
    public class FsContainer : BindingRoot, IDisposable {
        private readonly FsContainer parent;

        private IBindingResolver _bindingResolver;
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
            this._bindingResolver = new BindingResolver();
        }
        
        #region Resolve
        public T Resolve<T>() {
            return (T)Resolve(typeof (T));
        }

        public object Resolve(Type type)
        {
            foreach(var binding in this.GetBindings())
            {
                if(binding.Lifetime is PerResolveLifetimeManager)
                {
                    binding.Lifetime = new PerResolveLifetimeManager();
                }
            }

            var instance = _bindingResolver.Resolve(this.GetBindings(), type);

            if (!disposeManager.Contains(instance))
            {
                disposeManager.Add(instance);
            }
            
            return instance;
        }

        public IBindingResolver BindingResolver
        {
            get { return _bindingResolver; }
            set { _bindingResolver = value; }
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

        #region Dispose
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
        #endregion
    }
}
