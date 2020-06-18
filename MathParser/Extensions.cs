using System;
using System.Collections.Generic;
using System.Text;

namespace MathParser
{
    public static class Extensions
    {
        public static List<T> Add<T>(this List<T> list, T item)
        {
            list.Add(item);
            return list;
        }
    }
}
