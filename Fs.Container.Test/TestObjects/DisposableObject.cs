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
        private int disposeCount = 0;

        public bool WasDisposed
        {
            get { return wasDisposed; }
            set { wasDisposed = value; }
        }

        public int DisposeCount
        {
            get { return disposeCount; }
            set { disposeCount = value; }
        }

        public void Dispose()
        {
            wasDisposed = true;
            disposeCount += 1;
        }
    }
}
