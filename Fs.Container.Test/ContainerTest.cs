using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fs.Container.Test.TestObjects;

namespace Fs.Container.Test {
    internal interface ICustomerService {}
    internal class CustomerService : ICustomerService {
        private readonly IValidator _validator;
        public CustomerService(IValidator validator) {
            _validator = validator;
        }
    }

    internal interface IRule {
        string GetMessage();
        int GetNumber();
    }
    internal class Rule : IRule {
        private readonly string _message;
        private readonly int _number;

        private Rule() {}

        public Rule(string message) {
            _message = message;
        }

        public Rule(string message, int number) {
            _message = message;
            _number = number;
        }

        public string GetMessage() {
            return _message;
        }

        public int GetNumber() {
            return _number;
        }
    }

    interface IContractService {}
    internal class ContractService : IContractService {
        private readonly IContractRepository _contractRepository;
        public ContractService(IContractRepository contractRepository) {
            _contractRepository = contractRepository;
        }
    }

    interface IContractRepository {}
    internal class ContractRepository : IContractRepository {}

    interface IDocumentService {}
    internal class DocumentService : IDocumentService { }

    internal class ContractController {
        private readonly ICustomerService _customerService;
        private readonly IContractService _contractService;
        private readonly IDocumentService _documentService;

        public ContractController(ICustomerService customerService,
            IContractService contractService,
            IDocumentService documentService) {
            _customerService = customerService;
            _contractService = contractService;
            _documentService = documentService;
        }
    }

    [TestClass]
    public class ContainerTest {
        [TestMethod]
        public void TestNonExistingParametersDoesNotAffectToInstanceCreatingResolve()
        {
            // Act
            const string message = "Hello, world.";

            var container = new FsContainer();
            container.For<IRule>().Use<Rule>()
                .WithConstructorArgument("message", message)
                .WithConstructorArgument("messageException", message);

            // Arrange
            var instance = container.Resolve<IRule>();

            // Assert
            Assert.IsNotNull(instance, null);
            Assert.AreEqual(instance.GetMessage(), message);
        }

        [TestMethod]
        public void TestNextWithArgumentOverridePreviousResolve()
        {
            // Act
            const string msgFirst = "Hello, world.";
            const string msgLast = "Good night, world.";
            const int numFirst = 42;
            const int numLast = 7;

            var container = new FsContainer();
            container.For<IRule>().Use<Rule>()
                .WithConstructorArgument("message", msgFirst)
                .WithConstructorArgument("message", msgLast)
                .WithConstructorArgument("number", numFirst)
                .WithConstructorArgument("number", numLast);

            // Arrange
            var instance = container.Resolve<IRule>();

            // Assert
            Assert.IsNotNull(instance, null);
            Assert.AreEqual(instance.GetMessage(), msgLast);
            Assert.AreEqual(instance.GetNumber(), numLast);
        }

        [TestMethod]
        public void TestWithMultipleConstructorArgumentResolve()
        {
            // Act
            const string argument = "Hello, world.";
            const int number = 42;

            var container = new FsContainer();
            container.For<IRule>().Use<Rule>()
                .WithConstructorArgument("message", argument)
                .WithConstructorArgument("number", number);

            // Arrange
            var instance = container.Resolve<IRule>();

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

            var container = new FsContainer();
            container.For<IRule>().Use<Rule>()
                .WithConstructorArgument("message", argument);

            // Arrange
            var instance = container.Resolve<IRule>();

            // Assert
            Assert.IsNotNull(instance, null);
            Assert.AreEqual(instance.GetMessage(), argument);
            Assert.AreEqual(instance.GetNumber(), 0);
        }

        [TestMethod]
        public void TestRecursiveMultipleArgumentsResolve()
        {
            // Act
            var container = new FsContainer();
            container.For<ICustomerService>().Use<CustomerService>();
            container.For<IContractService>().Use<ContractService>();
            container.For<IDocumentService>().Use<DocumentService>();
            container.For<IValidator>().Use<Validator>();
            container.For<IContractRepository>().Use<ContractRepository>();

            // Arrange
            var instance = container.Resolve<ContractController>();

            // Assert
            Assert.AreNotEqual(instance, null);
        }

        [TestMethod]
        public void TestRecursiveSingleArgumentResolve()
        {
            // Act
            var container = new FsContainer();
            container.For<ICustomerService>().Use<CustomerService>();
            container.For<IValidator>().Use<Validator>();

            // Arrange
            var instance = container.Resolve<ICustomerService>();

            // Assert
            Assert.AreNotEqual(instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Type does not have a parameterless constructor")]
        public void TestMissingParameterlessConstructorResolve()
        {
            // Act
            var container = new FsContainer();

            // Arrange
            var instance = container.Resolve<Rule>();

            // Assert
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Type does not have a parameterless constructor")]
        public void TestMissingImplementationBindingResolve()
        {
            // Act
            var container = new FsContainer();

            // Arrange
            var validator = container.Resolve<IValidator>();

            // Assert
        }

        [TestMethod]
        public void TestForUseByConcreteResolve()
        {
            // Act
            var container = new FsContainer();
            container.For<Validator>().Use<Validator>();

            // Arrange
            var validator = container.Resolve<Validator>();

            // Assert
            Assert.AreNotEqual(validator, null);
        }

        [TestMethod]
        public void TestForUseByServiceResolve()
        {
            // Act
            var container = new FsContainer();
            container.For<IValidator>().Use<Validator>();

            // Arrange
            var validator = container.Resolve<IValidator>();

            // Assert
            Assert.AreNotEqual(validator, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNotAssignableInterfaceTypesThrowNotAssignableException()
        {
            // Act
            var container = new FsContainer();
            container.For<ILogger>().Use<Validator>();

            // Arrange
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNotAssignableBaseClassTypesThrowNotAssignableException()
        {
            // Act
            var container = new FsContainer();
            container.For<DbLogger>().Use<Logger>();

            // Arrange
        }

        [TestMethod]
        public void TestBaseClassInheritanceAreAssignableTypes()
        {
            // Act
            var container = new FsContainer();
            container.For<Logger>().Use<DbLogger>();

            // Arrange
            var logger = container.Resolve<Logger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsInstanceOfType(logger, typeof(DbLogger));
        }

        [TestMethod]
        public void TestInterfaceInheritanceAreAssignableTypes()
        {
            // Act
            var container = new FsContainer();
            container.For<ILogger>().Use<DbLogger>();

            // Arrange
            var logger = container.Resolve<ILogger>();

            // Assert
            Assert.IsNotNull(logger);
            Assert.IsInstanceOfType(logger, typeof(DbLogger));
        }
    }
}