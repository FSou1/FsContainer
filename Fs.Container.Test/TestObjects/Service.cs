namespace Fs.Container.Test.TestObjects
{
    internal class Service
    {
        public IMapper Mapper { get; }

        public Service(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}