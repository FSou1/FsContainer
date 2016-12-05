using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container {
    public class FsContainer {
        private readonly FsContainer parent;

        /// <summary>
        /// Create a default <see cref="FsContainer"/>
        /// </summary>
        public FsContainer() 
            : this(null) { }

        private FsContainer(FsContainer parent)
        {
            this.parent = parent;
        }

        internal List<BindingBuilder> _bindingBuilders = new List<BindingBuilder>();

        public T Resolve<T>() {
            return (T)Resolve(typeof (T));
        }

        public object Resolve(Type type) {
            var builder = this._bindingBuilders.FirstOrDefault(t => t._service == type);
            if (builder != null) {
                return CreateInstance(builder);
            }

            return CreateInstance(type);
        }

        #region Child container
        public FsContainer Parent {
            get { return parent; }
        }

        public FsContainer CreateChildContainer()
        {
            var child = new FsContainer(this);

            return child;
        }
        #endregion

        public object CreateInstance(BindingBuilder builder) {
            var concrete = builder._concrete;
            var arguments = builder._arguments ?? new Dictionary<string, object>();

            return CreateInstance(concrete, arguments);
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

        public BindingBuilder For<T>() {
            var type = typeof(T);
            
            var bindingBuilder = new BindingBuilder(type);
            _bindingBuilders.Add(bindingBuilder);

            return bindingBuilder;
        }
    }
}
