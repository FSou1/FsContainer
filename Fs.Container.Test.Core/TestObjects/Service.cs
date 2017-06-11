namespace Fs.Container.Test.Core.TestObjects
{
    public class Service
    {
        public IMapper Mapper { get; }

        public Service(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}