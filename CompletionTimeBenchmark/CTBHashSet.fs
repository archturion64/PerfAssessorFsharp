namespace CompletionTimeBenchmark
(*using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Diagnostics;

namespace CompletionTimeBenchmark
{
    public class CTBHashSet
    {
        private static Random rng = new Random();

        private static HashSet<Tuple<string, double>> GenerateHashSetTuple(uint size)
        {
            var container = new HashSet<Tuple<string, double>>();
            
            for (uint i = 0; i < size; i++)
            {
                container.Add(new Tuple<string, double>(i.ToString(), rng.NextDouble()));
            }
            return container;
        }

        private static HashSet<ValueTuple<string, double>> GenerateHashSetValueTuple(uint size)
        {
            var container = new HashSet<ValueTuple<string, double>>();
            
            for (uint i = 0; i < size; i++)
            {
                container.Add(new ValueTuple<string, double>(i.ToString(), rng.NextDouble()));
            }
            return container;
        }

        public static double DoBenchmarkTuple(uint size)
        {
            return CTBenchmark.RunBenchmark(() => {
                double cnt = 0;
                cnt += GenerateHashSetTuple(size).Sum(x => x.Item2);
            });
        }

        public static double DoBenchmarkValueTuple(uint size)
        {
            return CTBenchmark.RunBenchmark(() => {
                double cnt = 0;
                cnt += GenerateHashSetValueTuple(size).Sum(x => x.Item2);
            });
        }

    }
}*)