using EnterpriseMVVM.Data;
using EnterpriseMVVM.Data.Tests;
using EnterpriseMVVM.DesktopClient.ViewModels;
using EnterpriseMVVM.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseMVVM.DesktopClient.Tests
{
    [TestClass]
    public class MainViewModelTests : FunctionalTest
    {
        [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(MainViewModel).BaseType == typeof(ViewModel));
        }

        [TestMethod]
        public void ValidationErrorWhenCustomerNameIsNotGreaterThanOrEqualTo32Characters()
        {
            var viewModel = new MainViewModel
            {
                CustomerName = "B"
            };

            Assert.IsNotNull(viewModel["CustomerName"]);
        }

        [TestMethod]
        public void NoValidationErrorWhenCustomerNameMeetsAllRequiremenets()
        {
            var viewModel = new MainViewModel
            {
                CustomerName = "David Anderson"
            };

            Assert.IsNull(viewModel["CustomerName"]);

        }

        [TestMethod]
        public void ValidationErrorWhenCustomerNameIsNotProvided()
        {
            var viewModel = new MainViewModel
            {
                CustomerName = null
            };

            Assert.IsNotNull(viewModel["CustomerName"]);

        }

        [TestMethod]
        public void AddCustomerCommandCannotExecuteWhenFirstNameIsNotValid()
        {
            var viewModel = new MainViewModel
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
            var viewModel = new MainViewModel
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
            var viewModel = new MainViewModel
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
            var viewModel = new MainViewModel
            {
                FirstName = "David",
                LastName = "Anderson",
                Email = "noreply@northwind.com"
            };

            viewModel.AddCommand.Execute(null);

            Assert.IsTrue(viewModel.Customers.Count == 1);
        }

        [TestMethod]
        public void GetCustomerListCommandPopulatesCustomersProperty()
        {
            using (var context = new BusinessContext())
            {
                context.AddNewCustomer(new Customer { FirstName = "1@1.com", LastName = "1", Email = "A" });
                context.AddNewCustomer(new Customer { FirstName = "2@2.com", LastName = "2", Email = "B" });
                context.AddNewCustomer(new Customer { FirstName = "3@3.com", LastName = "3", Email = "C" });

                var viewModel = new MainViewModel(context);

                viewModel.GetCustomerListCommand.Execute(null);

                Assert.IsTrue(viewModel.Customers.Count == 3);
            }

        }

        [TestMethod]
        public void SaveCommand_UpdatesSelectedCustomerFirstName()
        {
            using (var context = new BusinessContext())
            {
                // Arrange
                context.AddNewCustomer(new Customer { Email = "1@1.com", FirstName = "1", LastName = "A" });

                var viewModel = new MainViewModel(context);

                viewModel.GetCustomerListCommand.Execute(null);
                viewModel.SelectedCustomer = viewModel.Customers.First();

                // Act
                viewModel.SelectedCustomer.FirstName = "newValue";
                viewModel.SaveCustomerCommand.Execute(null);

                // Assert
                var customer = context.DataContext.Customers.Single();
                context.DataContext.Entry(customer).Reload();
                Assert.AreEqual(viewModel.SelectedCustomer.FirstName, customer.FirstName);
            }
        }

    }
}
