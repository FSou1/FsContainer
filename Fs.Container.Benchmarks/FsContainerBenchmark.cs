using Fs.Container.Benchmarks.Scenarios;
using Fs.Container.Core;
using Fs.Container.Core.Lifetime;

namespace Fs.Container.Benchmarks {
    public class FsContainerBenchmark : IContainerBenchmark
    {
        public static readonly FsContainerBenchmark Instance = new FsContainerBenchmark();

        private readonly IFsContainer container = new FsContainer();
        public FsContainerBenchmark()
        {
            container.For<ISingleton>().Use<Singleton>(new ContainerControlledLifetimeManager());
            container.For<ITransient>().Use<Transient>(new TransientLifetimeManager());
        }

        public ISingleton ResolveSingleton() => container.Resolve<ISingleton>();

        public ITransient ResolveTransient() => container.Resolve<ITransient>();
    }

    public interface IContainerBenchmark
    {
        ISingleton ResolveSingleton();
        ITransient ResolveTransient();
    }
}