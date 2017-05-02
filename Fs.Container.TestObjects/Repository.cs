namespace Fs.Container.TestObjects
{
    public class Repository : IRepository
    {
        public string ConnectionString { get; }

        public Repository(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }

    public interface IRepository
    {
        string ConnectionString { get; }
    }
}