using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace Fs.Ninject.Test
{
    [TestClass]
    public class MultipleCtorArgsLengthTest
    {
        internal interface IRule
        {
            string GetMessage();
            int GetNumber();
        }
        internal class Rule : IRule
        {
            private readonly string _message;
            private readonly int _number;
            
            private Rule() { }
            public Rule(string message)
            {
                _message = message;
            }
            
            public Rule(string message, int number)
            {
                _message = message;
                _number = number;
            }

            public string GetMessage()
            {
                return _message;
            }

            public int GetNumber()
            {
                return _number;
            }
        }

        [TestMethod]
        public void TestWithMultipleConstructorArgumentResolve()
        {
            // Act
            const string argument = "Hello, world.";
            const int number = 42;
        
            var container = new StandardKernel();
            container.Bind<IRule>().To<Rule>()
                .WithConstructorArgument("message", argument)
                .WithConstructorArgument("number", number);

            // Arrange
            var instance = container.Get<IRule>();

            // Assert
            Assert.IsNotNull(instance, null);
            Assert.AreEqual(instance.GetMessage(), argument);
            Assert.AreEqual(instance.GetNumber(), number);
        }

        [TestMethod]
        public void TestWithSingleConstructorArgumentResolve()
        {
            // Act
            const string argument = "Hello, world.";
            
            var container = new StandardKernel();
            container.Bind<IRule>().To<Rule>()
                .WithConstructorArgument("message", argument);

            // Arrange
            var instance = container.Get<IRule>();

            // Assert
            Assert.IsNotNull(instance, null);
            Assert.AreEqual(instance.GetMessage(), argument);
            Assert.AreEqual(instance.GetNumber(), 0);
        }
    }
}
