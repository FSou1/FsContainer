namespace Fs.Container.Core.Lifetime {
    public interface ILifetimeManager {
        object GetValue();
        void SetValue(object newValue);
        void RemoveValue();
    }
}
