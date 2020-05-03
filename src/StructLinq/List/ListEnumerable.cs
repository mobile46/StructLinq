﻿using System.Collections.Generic;
using StructLinq.Array;
using StructLinq.Utils;

namespace StructLinq.List
{
    public struct ListEnumerable<T> : IStructEnumerable<T, ArrayStructEnumerator<T>>
    {
        private List<T> list;
        public ListEnumerable(List<T> list)
        {
            this.list = list;
        }

        public ArrayStructEnumerator<T> GetEnumerator()
        {
            var layout = Unsafe.As<List<T>, ListLayout<T>>(ref list);
            return new ArrayStructEnumerator<T>(layout.Items, list.Count);
        }
    }
}