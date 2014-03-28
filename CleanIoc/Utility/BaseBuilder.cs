using System;

namespace CleanIoc.Utility
{
    public abstract class BaseBuilder<T>
    {
        internal bool WasBuild { get; private set; }

        public T Build()
        {
            EnsureWasNotBuilt();
            T result = OnBuild();
            WasBuild = true;
            return result;
        }

        protected abstract T OnBuild();

        protected void EnsureWasNotBuilt()
        {
            if (WasBuild)
                throw new InvalidOperationException(WasAlreadyBuiltExceptionMessage);
        }

        protected virtual string WasAlreadyBuiltExceptionMessage
        {
            get { return typeof (T).Name + " was already built"; } 
        }
    }
}
