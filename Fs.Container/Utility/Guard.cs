using Fs.Container.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fs.Container.Utility
{
    public static class Guard
    {
        public static void ArgumentNotNull(object argumentValue,
                                           string argumentName)
        {
            if(argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void ArgumentNotNullOrEmpty(string argumentValue, 
                                                  string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if(argumentValue.Length == 0)
            {
                throw new ArgumentException(Resources.ArgumentMustNotBeEmpty, argumentName);
            }
        }
    }
}
