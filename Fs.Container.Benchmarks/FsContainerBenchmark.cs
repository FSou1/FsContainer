using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Fs.Container.Benchmarks.Scenarios;
using Fs.Container.Core;
using Fs.Container.Core.Lifetime;

namespace Fs.Container.Benchmarks {
    [Config(typeof(Config))]
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    public class FsContainerBenchmark : IContainerBenchmark
    {
        public static readonly FsContainerBenchmark Instance = new FsContainerBenchmark();

        private readonly IFsContainer container = new FsContainer();

        public FsContainerBenchmark()
        {
            container.For<ISingleton>().Use<Singleton>(new ContainerControlledLifetimeManager());
            container.For<ITransient>().Use<Transient>(new TransientLifetimeManager());
            container.For<ICombined>().Use<Combined>(new TransientLifetimeManager());
        }

        [Benchmark]
        public ISingleton ResolveSingleton() => container.Resolve<ISingleton>();

        [Benchmark]
        public ITransient ResolveTransient() => container.Resolve<ITransient>();

        [Benchmark]
        public ICombined ResolveCombined() => container.Resolve<ICombined>();
    }

    public interface IContainerBenchmark
    {
        ISingleton ResolveSingleton();
        ITransient ResolveTransient();

    }
}