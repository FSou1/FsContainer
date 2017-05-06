using System.Threading;
using System.Threading.Tasks;

namespace Fs.Container.TestObjects
{
    public class Repository : IRepository
    {
        public string ConnectionString { get; }

        public static Task<IRepository> CreateInstanceAsync(string connectionString) {
            return Task.FromResult<IRepository>(new Repository(connectionString));
        }

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