#if CLR_2_0
using System;
using System.Collections.Generic;

namespace Celigo.Linq
{
    /// <summary>
    /// Represents a method that takes a single parameter and return a value.
    /// </summary>
    public delegate R Func<T, R>(T arg);
    /// <summary>
    /// Represents a method that takes no parameters and return a value.
    /// </summary>
    public delegate T Func<T>();

    class Extensions
    {
        public static T[] ToArray<T, K>(Dictionary<K, T> dictionary)
        {
            T[] array = new T[dictionary.Count];
            int i = 0;
            foreach (var value in dictionary.Values)
            {
                array[i++] = value;
            }
            return array;
        }
    }
}
#endif
