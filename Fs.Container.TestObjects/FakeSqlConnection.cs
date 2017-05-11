using System.Threading.Tasks;

namespace Fs.Container.TestObjects {
    public class FakeSqlConnection : IFakeDbConnection {
        private bool isOpen = false;

        public bool IsOpen {
            get => isOpen;
            set => isOpen = value;
        }

        public string ConnectionString { get; }

        public FakeSqlConnection(string connectionString) {
            ConnectionString = connectionString;
        }

        public void EnsureOpen() {
            if (!IsOpen)
            {
                IsOpen = true;
            }
        }

        public Task EnsureOpenAsync() {
            if (!IsOpen)
            {
                IsOpen = true;
            }

            return Task.CompletedTask;
        }
    }

    public interface IFakeDbConnection {
        bool IsOpen { get; }
        string ConnectionString { get; }

        void EnsureOpen();

        Task EnsureOpenAsync();
    }
}