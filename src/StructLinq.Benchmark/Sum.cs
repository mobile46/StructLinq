﻿using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using StructLinq.IEnumerable;
using StructLinq.Range;

namespace StructLinq.Benchmark
{
    [MemoryDiagnoser]
    public class Sum
    {
        private readonly IEnumerable<int> sysRange;
        private readonly IStructEnumerable<int, GenericEnumerator<int>> convertRange;
        private const int Count = 10000;
        private readonly IStructEnumerable<int, RangeEnumerator> structRange;
        public Sum()
        {
            sysRange = Enumerable.Range(0, Count);
            convertRange = Enumerable.Range(0, Count).ToStructEnumerable();
            structRange = StructEnumerable.Range(0, Count);
        }


        [Benchmark(Baseline = true)]
        public int ForSum()
        {
            int sum = 0;
            for (int i = 0; i < Count; i++)
            {
                sum += i;
            }
            return sum;
        }

        [Benchmark]
        public int SysSum()
        {
            return sysRange.Sum();
        }

        [Benchmark]
        public int StructSum()
        {
            return structRange.Sum();
        }

        [Benchmark]
        public int ConvertSum()
        {
            return convertRange.Sum();
        }

    }
}