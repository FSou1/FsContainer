using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Lifetime {
    public interface ILifetimeManager {
        object GetValue();
        void SetValue(object newValue);
        void RemoveValue();
    }
}
