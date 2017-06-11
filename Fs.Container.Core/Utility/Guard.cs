using System;
using System.Reflection;

namespace Fs.Container.Core.Utility
{
    public static class Guard
    {
        /// <summary>
        /// Throws exception if the argument is null
        /// </summary>
        /// <param name="argumentValue"></param>
        /// <param name="argumentName"></param>
        public static void ArgumentNotNull(object argumentValue,
                                           string argumentName)
        {
            if(argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Throws an exception if the string argument is null or empty
        /// </summary>
        /// <param name="argumentValue"></param>
        /// <param name="argumentName"></param>
        public static void ArgumentNotNullOrEmpty(string argumentValue, 
                                                  string argumentName)
        {
            ArgumentNotNull(argumentValue, argumentName);

            if(argumentValue.Length == 0)
            {
                throw new ArgumentException(Resources.ArgumentMustNotBeEmpty, argumentName);
            }
        }

        /// <summary>
        /// Verify that concrete is assignable from service (interfaces 
        /// are implemented, or classes exist in the base class hierarchy)
        /// </summary>
        /// <param name="service"></param>
        /// <param name="concrete"></param>
        public static void TypeIsAssignable(Type service, Type concrete)
        {
            ArgumentNotNull(service, nameof(service));
            ArgumentNotNull(concrete, nameof(concrete));

            if(!service.GetTypeInfo().IsAssignableFrom(concrete.GetTypeInfo()))
            {
                throw new ArgumentException(string.Format(
                    Resources.TypesAreNotAssignable, 
                    service, 
                    concrete));
            }
        }
    }
}
