using System;
using System.Collections.Generic;

namespace CleanIoc.Utility
{
    class TypeComparer : IComparer<Type>
    {
        public int Compare(Type x, Type y)
        {
            int result = x.GUID.CompareTo(y.GUID);

            if (result == 0 && x != y)
                result = CompareNamesFor(x, y);

            return result;
        }

        private static int CompareNamesFor(Type x, Type y)
        {
            return String.Compare(UniqueName(x), UniqueName(y), StringComparison.Ordinal);
        }

        private static string UniqueName(Type type)
        {
            return type.Assembly.FullName + type.FullName;
        }
    }
}
