namespace Fs.Container.Test.TestObjects
{
    internal class Controller
    {
        public Service Service { get; }
        public IMapper Mapper { get; }

        public Controller(Service service, IMapper mapper)
        {
            Service = service;
            Mapper = mapper;
        }
    }
}