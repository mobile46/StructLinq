﻿using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using StructLinq.IEnumerable;
using StructLinq.Range;
using StructLinq.Select;

namespace StructLinq.Benchmark
{
    [MemoryDiagnoser]
    public class Select
    {
        private readonly IEnumerable<double> sysRange;
        private readonly IStructEnumerable<double, SelectEnumerator<int, double, RangeEnumerator, StructFunction<int, double>>> delegateRange;
        private readonly IStructEnumerable<double, SelectEnumerator<int, double, GenericEnumerator<int>, StructFunction<int, double>>> convertRange;
        private readonly CountAction<double>[] countActions = new CountAction<double>[1];
        private const int Count = 10000;
        private readonly IStructEnumerable<double, SelectEnumerator<int, double, RangeEnumerator, MultFunction>> structRange;
        public Select()
        {
            sysRange = Enumerable.Range(0, Count).Select(x=> x * 2.0);
            delegateRange = StructEnumerable.Range(0, Count).Select(x=> x * 2.0);
            convertRange = Enumerable.Range(0, Count).ToStructEnumerable().Select(x=> x * 2.0);
            var multFunction = new MultFunction();
            structRange = StructEnumerable.Range(0, Count).Select(in multFunction, Id<double>.Value);
        }

        [Benchmark(Baseline = true)]
        public int SysSelect()
        {
            int count = 0;
            foreach (var i in sysRange)
            {
                count++;
            }
            return count;
        }

        [Benchmark]
        public int DelegateSelect()
        {
            int count = 0;
            delegateRange.ForEach(i => count++);
            return count;
        }

        [Benchmark]
        public int StructSelect()
        {
            ref CountAction<double> countAction = ref countActions[0];
            countAction.Count = 0;
            structRange.ForEach(ref countAction);
            return countAction.Count;
        }

        [Benchmark]
        public int ConvertSelect()
        {
            ref CountAction<double> countAction = ref countActions[0];
            countAction.Count = 0;
            convertRange.ForEach(ref countAction);
            return countAction.Count;
        }
    }

    struct MultFunction : IFunction<int, double>
    {
        public readonly double Eval(in int element)
        {
            return element * 2.0;
        }
    }

}