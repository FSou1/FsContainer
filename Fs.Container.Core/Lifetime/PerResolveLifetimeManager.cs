namespace Fs.Container.Core.Lifetime
{
    public class PerResolveLifetimeManager : ILifetimeManager
    {
        private readonly object value;

        public PerResolveLifetimeManager()
        {
        }

        /// <summary>
        /// Construct new manager that stores passed value
        /// </summary>
        /// <param name="value"></param>
        internal PerResolveLifetimeManager(object value)
        {
            this.value = value;
        }

        public object GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Set value occures only with the internal constructor usage
        /// </summary>
        /// <param name="newValue"></param>
        public void SetValue(object newValue)
        {
        }
        
        public void RemoveValue()
        {
        }
    }
}