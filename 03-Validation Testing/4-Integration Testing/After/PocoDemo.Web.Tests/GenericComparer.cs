using System;
using System.Collections.Generic;

namespace PocoDemo.Web.Tests
{
    public class GenericComparer<T> : IEqualityComparer<T> 
        where T : class
    {
        private readonly Func<T, T, bool> _comparer;

        public GenericComparer(Func<T, T, bool> comparer)
        {
            _comparer = comparer;
        }
        public bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }
        public int GetHashCode(T x)
        {
            return x.GetHashCode();
        }
    }
}