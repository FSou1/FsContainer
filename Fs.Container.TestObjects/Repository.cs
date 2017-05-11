using System.Threading.Tasks;

namespace Fs.Container.TestObjects
{
    public class Repository : IRepository
    {
        public IFakeDbConnection Connection { get; }

        public static async Task<IRepository> CreateInstanceAsync(IFakeDbConnection connection) {
            await connection.EnsureOpenAsync();
            return new Repository(connection);
        }

        public Repository(IFakeDbConnection connection) {
            Connection = connection;
        }
    }

    public interface IRepository
    {
        IFakeDbConnection Connection { get; }
    }
}