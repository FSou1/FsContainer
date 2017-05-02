using Fs.Container.Dispose;
using Fs.Container.Syntax;
using System;
using System.Linq;
using Fs.Container.Resolve;

namespace Fs.Container {
    public class FsContainer : BindingRoot, IFsContainer {
        private readonly FsContainer parent;

        private IBindingResolver _bindingResolver;
        private DisposeManager _disposeManager;

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
                parent._disposeManager.Add(this);
            }
            
            this._disposeManager = new DisposeManager();
            this._bindingResolver = new BindingResolver();
        }
        
        #region Resolve
        public T Resolve<T>() {
            return (T)Resolve(typeof (T));
        }

        public object Resolve(Type type)
        {
            var instance = _bindingResolver.Resolve(this, GetBindings(), type);

            if (!_disposeManager.Contains(instance))
            {
                _disposeManager.Add(instance);
            }
            
            return instance;
        }

        public IBindingResolver BindingResolver
        {
            get => _bindingResolver;
            set => _bindingResolver = value;
        }
        #endregion

        #region Child container
        public IFsContainer Parent => parent;

        public IFsContainer CreateChildContainer()
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
                if(_disposeManager != null)
                {
                    _disposeManager.Dispose();
                    _disposeManager = null;

                    if(parent?._disposeManager != null)
                    {
                        parent._disposeManager.Remove(this);
                    }
                }
            }
        }
        #endregion
    }
}
