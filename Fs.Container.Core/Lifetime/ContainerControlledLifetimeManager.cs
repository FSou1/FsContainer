using System;

namespace Fs.Container.Core.Lifetime {
    public class ContainerControlledLifetimeManager : ILifetimeManager, IDisposable {
        private object value;

        /// <summary>
        /// Return stored value
        /// </summary>
        /// <returns></returns>
        public object GetValue() {
            return this.value;
        }

        /// <summary>
        /// Remove value using dispose of manager
        /// </summary>
        public void RemoveValue() {
            this.Dispose();
        }

        /// <summary>
        /// Store value for retrieval later
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(object newValue) {
            this.value = newValue;
        }

        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Standard Dispose pattern implementation
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (this.value != null) {
                if (this.value is IDisposable) {
                    ((IDisposable)this.value).Dispose();
                }
                this.value = null;
            }
        }
    }
}
