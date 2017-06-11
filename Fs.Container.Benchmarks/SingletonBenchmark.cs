using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Fs.Container.Benchmarks.Scenarios;

namespace Fs.Container.Benchmarks {
    [Config(typeof(Config))]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    public class SingletonBenchmark
    {
        [Benchmark]
        public ISingleton FsContainer() => FsContainerBenchmark.Instance.ResolveSingleton();
    }
}