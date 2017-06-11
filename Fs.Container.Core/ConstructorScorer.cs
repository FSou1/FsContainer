using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fs.Container.Core
{
    public class ConstructorScorer
    {
        private readonly Type _concrete;
        private readonly IDictionary<string, object> _constructorArguments;

        public ConstructorScorer(Type concrete, IDictionary<string, object> constructorArguments) {
            _concrete = concrete;
            _constructorArguments = constructorArguments;
        }

        public ConstructorInfo GetConstructor() {
            var constructor = _concrete
                .GetConstructors()
                .OrderByDescending(Score)
                .FirstOrDefault();
            
            if (constructor == null) {
                throw new Exception(Resources.ParameterlessConstructorNotFound);
            }

            return constructor;
        }

        public int Score(ConstructorInfo constructor) {
            var parameters = constructor.GetParameters();

            return parameters.Count(p => _constructorArguments.ContainsKey(p.Name));
        }
    }
}
