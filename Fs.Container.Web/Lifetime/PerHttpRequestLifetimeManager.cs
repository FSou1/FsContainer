using Fs.Container.Core.Lifetime;
using System;
using System.Web;

namespace Fs.Container.Web.Lifetime
{
    public class PerHttpRequestLifetimeManager : ILifetimeManager
    {
        private readonly Guid _key = Guid.NewGuid();

        public object GetValue() 
            => HttpContext.Current.Items[_key];

        public void SetValue(object newValue)
            => HttpContext.Current.Items[_key] = newValue;        

        public void RemoveValue()
        {
        }
    }
}
