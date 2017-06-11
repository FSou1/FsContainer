using System;
using BenchmarkDotNet.Running;

namespace Fs.Container.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher switcher = new BenchmarkSwitcher(new[] {
                typeof(SingletonBenchmark),
                typeof(TransientBenchmark)
            });
            switcher.Run();
        }
    }
}