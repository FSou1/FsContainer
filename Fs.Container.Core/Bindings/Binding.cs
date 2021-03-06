﻿using System;
using System.Collections.Generic;
using Fs.Container.Core.Lifetime;

namespace Fs.Container.Core.Bindings
{
    public class Binding : IBinding, IDisposable
    {
        public Type Service { get; private set; }
        public IBindingConfiguration BindingConfiguration { get; private set; }

        public Binding(Type service)
        {
            this.Service = service;
            this.BindingConfiguration = new BindingConfiguration();
        }

        public Binding(Type service, Type concrete, IDictionary<string, object> arguments, ILifetimeManager lifetime)
             : this(service)
        {
            this.Concrete = concrete;
            this.Arguments = arguments;
            this.Lifetime = lifetime;
        }

        public Type Concrete
        {
            get => this.BindingConfiguration.Concrete;
            set => this.BindingConfiguration.Concrete = value;
        }

        public ILifetimeManager Lifetime
        {
            get => this.BindingConfiguration.Lifetime;
            set => this.BindingConfiguration.Lifetime = value;
        }

        public IDictionary<string, object> Arguments
        {
            get => this.BindingConfiguration.Arguments;
            set => this.BindingConfiguration.Arguments = value;
        }

        public Func<IFsContainer, object> FactoryFunc
        {
            get => this.BindingConfiguration.FactoryFunc;
            set => this.BindingConfiguration.FactoryFunc = value;
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
            if (this.Lifetime != null)
            {
                if (this.Lifetime is IDisposable)
                {
                    ((IDisposable)this.Lifetime).Dispose();
                }
                this.Lifetime = null;
            }
        }
    }
}
