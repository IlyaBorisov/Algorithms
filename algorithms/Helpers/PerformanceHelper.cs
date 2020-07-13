using Algorithms.Helpers;
using Algorithms.Sorting;
using Algorithms.DivideAndRule;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Helpers
{
    public static class PerformanceHelper
    {
        public static void GetPerformanceOfAlgorithm<Tin>(Tin arr, Action<Tin> algorithm)
        {
            var timer = new Stopwatch();
            timer.Start();
            algorithm(arr);
            timer.Stop();
            Console.WriteLine($"{algorithm.Method.Name}: {timer.ElapsedTicks} ticks, {timer.ElapsedMilliseconds} ms.");
        }

        public static void GetPerformanceOfSorting<Tin>(Tin[] arr, Func<Tin[], Tin[]> algorithm) where Tin : IComparable<Tin>
        {
            var copyArr = new Tin[arr.Length];
            Array.Copy(arr, copyArr, arr.Length);
            var timer = new Stopwatch();
            timer.Start();
            var outArr = algorithm(copyArr);
            timer.Stop();
            outArr.Show(true, false, arr);
            Console.WriteLine($"{algorithm.Method.Name}: {timer.ElapsedTicks} ticks, {timer.ElapsedMilliseconds} ms.");
        }
    }
}
