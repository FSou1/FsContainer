namespace Fs.Container.TestObjects
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