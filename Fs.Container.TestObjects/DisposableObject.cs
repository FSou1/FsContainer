using System;

namespace Fs.Container.TestObjects
{
    public class DisposableObject : IDisposable
    {
        private bool wasDisposed = false;
        private int disposeCount = 0;

        public bool WasDisposed
        {
            get => wasDisposed;
            set => wasDisposed = value;
        }

        public int DisposeCount
        {
            get => disposeCount;
            set => disposeCount = value;
        }

        public void Dispose()
        {
            wasDisposed = true;
            disposeCount += 1;
        }
    }
}
