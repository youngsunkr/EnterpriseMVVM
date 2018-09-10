using EnterpriseMVVM.Data;
using EnterpriseMVVM.Data.Tests;
using EnterpriseMVVM.DesktopClient.ViewModels;
using EnterpriseMVVM.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;

namespace EnterpriseMVVM.DesktopClient.Tests
{
    [TestClass]
    public class MainViewModelTests
    {
        private Mock<IBusinessContext> mock;
        private List<Customer> store;

        [TestInitialize]
        public void TestInitialize()
        {
            store = new List<Customer>();

            mock = new Mock<IBusinessContext>();
            mock.Setup(m => m.GetCustomerList()).Returns(store);
            mock.Setup(m => m.CreateCustomer(It.IsAny<Customer>())).Callback<Customer>(customer => store.Add(customer));
            mock.Setup(m => m.DeleteCustomer(It.IsAny<Customer>())).Callback<Customer>(customer => store.Remove(customer));
            mock.Setup(m => m.UpdateCustomer(It.IsAny<Customer>())).Callback<Customer>(customer =>
                                                                                                {
                                                                                                    int i = store.IndexOf(customer);
                                                                                                    store[i] = customer;
                                                                                                });

        }



        [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(MainViewModel).BaseType == typeof(ViewModel));
        }

        [TestMethod]
        public void ValidationErrorWhenCustomerNameIsNotGreaterThanOrEqualTo32Characters()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                CustomerName = "B"
            };

            Assert.IsNotNull(viewModel["CustomerName"]);
        }

        [TestMethod]
        public void NoValidationErrorWhenCustomerNameMeetsAllRequiremenets()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                CustomerName = "David Anderson"
            };

            Assert.IsNull(viewModel["CustomerName"]);

        }

        [TestMethod]
        public void ValidationErrorWhenCustomerNameIsNotProvided()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                CustomerName = null
            };

            Assert.IsNotNull(viewModel["CustomerName"]);

        }

        [TestMethod]
        public void AddCustomerCommandCannotExecuteWhenFirstNameIsNotValid()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                SelectedCustomer = new Customer()
                {
                    FirstName = null,
                    LastName = "Anderson",
                    Email = "noreply@northwind.com"
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCustomerCommandCannotExecuteWhenLastNameIsNotValid()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                SelectedCustomer = new Customer()
                {
                    FirstName = "David",
                    LastName = null,
                    Email = "noreply@northwind.com"
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCustomerCommandCannotExecuteWhenEmailIsNotValid()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                SelectedCustomer = new Customer()
                {
                    FirstName = "David",
                    LastName = "Anderson",
                    Email = null
                }
            };

            Assert.IsFalse(viewModel.AddCommand.CanExecute(null));
        }

        [TestMethod]
        public void AddCustomerCommandAddsCustomerToCustomersCollectionWhenExecutedSuccessfully()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                SelectedCustomer = new Customer()
                {
                    FirstName = "David",
                    LastName = "Anderson",
                    Email = "noreply@northwind.com"
                }
            };

            viewModel.AddCommand.Execute(null);

            Assert.IsTrue(viewModel.Customers.Count == 1);
        }

        [TestMethod]
        public void CanModify_ShouldEqualFalseWhenSelectedCustomerIsNull()
        {
            var viewModel = new MainViewModel(mock.Object) { SelectedCustomer = null };
            Assert.IsFalse(viewModel.CanModify);
        }

        [TestMethod]
        public void CanModify_ShouldEqualTrueWhenSelectedCustomerIsNotNull()
        {
            var viewModel = new MainViewModel(mock.Object) { SelectedCustomer = new Customer() };
            Assert.IsTrue(viewModel.CanModify);
        }

        [TestMethod]
        public void GetCustomerListCommandPopulatesCustomersProperty()
        {
            mock.Object.CreateCustomer(new Customer { FirstName = "1@1.com", LastName = "1", Email = "A" });
            mock.Object.CreateCustomer(new Customer { FirstName = "2@2.com", LastName = "2", Email = "B" });
            mock.Object.CreateCustomer(new Customer { FirstName = "3@3.com", LastName = "3", Email = "C" });

            var viewModel = new MainViewModel(mock.Object);

            viewModel.GetCustomerListCommand.Execute(null);

            Assert.IsTrue(viewModel.Customers.Count == 3);

        }

        [TestMethod]
        public void GetCustomerListCommand_SelectedCustomerIsSetToNullWhenExecuted()
        {
            var viewModel = new MainViewModel(mock.Object)
            {
                SelectedCustomer = new Customer
                {
                    Id = 1,
                    FirstName = "David",
                    LastName = "Anderson",
                    Email = "noreply@northwind.com"
                }
            };

            viewModel.AddCommand.Execute(null);

            Assert.IsTrue(viewModel.Customers.Count == 1);
        }

        [TestMethod]
        public void SaveCommand_InvokesIBusinessContextUpdateCustomerMethod()
        {
            // Arrange
            mock.Object.CreateCustomer(new Customer { Email = "1@1.com", FirstName = "1", LastName = "A" });

            var viewModel = new MainViewModel(mock.Object);

            viewModel.GetCustomerListCommand.Execute(null);
            viewModel.SelectedCustomer = viewModel.Customers.First();

            // Act
            viewModel.UpdateCommand.Execute(null);

            // Assert
            mock.Verify(m => m.UpdateCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public void SaveCommand_UpdatesSelectedCustomerFirstName()
        {
            using (var context = new BusinessContext())
            {
                // Arrange
                context.CreateCustomer(new Customer { Email = "1@1.com", FirstName = "1", LastName = "A" });

                var viewModel = new MainViewModel(context);

                viewModel.GetCustomerListCommand.Execute(null);
                viewModel.SelectedCustomer = viewModel.Customers.First();

                // Act
                viewModel.SelectedCustomer.FirstName = "newValue";
                viewModel.UpdateCommand.Execute(null);

                // Assert
                var customer = context.DataContext.Customers.Single();
                context.DataContext.Entry(customer).Reload();
                Assert.AreEqual(viewModel.SelectedCustomer.FirstName, customer.FirstName);
            }
        }

        [TestMethod]
        public void DeleteCommand_InvokesIBusinessContextDeleteCustomerMethod()
        {
            // Arrange
            mock.Object.CreateCustomer(new Customer { Email = "1@1.com", FirstName = "1", LastName = "A" });

            var viewModel = new MainViewModel(mock.Object);

            viewModel.GetCustomerListCommand.Execute(null);
            viewModel.SelectedCustomer = viewModel.Customers.First();

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert
            mock.Verify(m => m.DeleteCustomer(It.IsAny<Customer>()), Times.Once);
        }

        [TestMethod]
        public void DeleteCommand_SelectedCustomerIsSetToNull()
        {
            // Arrange
            mock.Object.CreateCustomer(new Customer { Email = "1@1.com", FirstName = "1", LastName = "A" });

            var viewModel = new MainViewModel(mock.Object);

            viewModel.GetCustomerListCommand.Execute(null);
            viewModel.SelectedCustomer = viewModel.Customers.First();

            // Act
            viewModel.DeleteCommand.Execute(null);

            // Assert
            Assert.IsNull(viewModel.SelectedCustomer);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForCanModifyWhenSelectedCustomerPropertyHasChanged()
        {
            var viewModel = new MainViewModel(mock.Object);

            bool eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
                                         {
                                             if (e.PropertyName == "CanModify")
                                                 eventRaised = true;
                                         };

            viewModel.SelectedCustomer = null;

            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void PropertyChanged_IsRaisedForSelectedCustomerWhenSelectedCustomerPropertyHasChanged()
        {
            var viewModel = new MainViewModel(mock.Object);

            bool eventRaised = false;

            viewModel.PropertyChanged += (sender, e) =>
                                         {
                                             if (e.PropertyName == "SelectedCustomer")
                                                 eventRaised = true;
                                         };

            viewModel.SelectedCustomer = null;

            Assert.IsTrue(eventRaised);
        }
    }
}
