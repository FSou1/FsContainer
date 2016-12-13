using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Test.TestObjects
{
    public class DisposableObject : IDisposable
    {
        private bool wasDisposed = false;

        public bool WasDisposed
        {
            get { return wasDisposed; }
            set { wasDisposed = value; }
        }

        public void Dispose()
        {
            wasDisposed = true;
        }
    }
}
