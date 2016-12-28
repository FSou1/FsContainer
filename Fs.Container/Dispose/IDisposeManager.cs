using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Dispose
{
    /// <summary>
    /// Represents a dispose manager
    /// <remarks>
    /// When the manager is disposed, any objects that this manager holds
    /// which implement IDisposable are also disposed.
    /// </remarks>
    /// </summary>
    public interface IDisposeManager : IDisposable
    {
        /// <summary>
        /// Add an object to the dispose container
        /// </summary>
        /// <param name="item"></param>
        void Add(object item);

        /// <summary>
        /// Check if an object already contains in the dispose container
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool Contains(object item);

        /// <summary>
        /// Remove an object from the dispose container
        /// </summary>
        /// <param name="item"></param>
        void Remove(object item);
    }
}
