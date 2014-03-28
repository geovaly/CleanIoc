using System;
using System.Collections.Generic;

namespace CleanIoc.Core
{
    class Disposer : IDisposable
    {
        private bool _isDisposed;
        private readonly Stack<IDisposable> _items = new Stack<IDisposable>();
        private readonly object _syncRoot = new object();

        public void Dispose()
        {
            if (_isDisposed)
                return;

            lock (_syncRoot)
            {
                if (_isDisposed)
                    return;

                _isDisposed = true;
                DisposeInstances();
            }
        }

        public void AddInstanceForDisposal(IDisposable instance)
        {
            lock (_syncRoot)
            {
                CheckNotDisposed();
                _items.Push(instance);
            }
        }

        private void DisposeInstances()
        {
            while (_items.Count > 0)
            {
                var item = _items.Pop();
                item.Dispose();
            }
        }

        protected void CheckNotDisposed()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }
    }
}
