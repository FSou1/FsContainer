using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Dispose
{
    public class DisposeManager : IDisposeManager
    {
        public void Add(object item)
        {
            items.Add(item);
        }
        
        public bool Contains(object item)
        {
            return items.Contains(item);
        }

        public void Remove(object item)
        {
            if (!items.Contains(item))
                return;

            items.Remove(item);
        }

        /// <summary>
        /// Releases the resources that <see cref="DisposeManager"/> holds. 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // Doesn't matter, but shuts up FxCop
        }

        /// <summary>
        /// Releases the resources that <see cref="DisposeManager"/> holds.
        /// </summary>
        /// <param name="disposing">
        /// true to release managed and unmanaged resources; false to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                var itemsCopy = new List<object>(items);
                itemsCopy.Reverse();

                foreach (object o in itemsCopy)
                {
                    var d = o as IDisposable;

                    if (d != null)
                    {
                        d.Dispose();
                    }
                }

                items.Clear();
            }
        }

        private readonly IList<object> items = new List<object>();
    }
}
