using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Lifetime {
    public class TransientLifetimeManager : ILifetimeManager {
        /// <summary>
        /// Return null to create new instance per resolve
        /// </summary>
        /// <returns></returns>
        public object GetValue() 
        {
            return null;
        }

        /// <summary>
        /// Do not store any of created instances to resolve a new one per resolve
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
