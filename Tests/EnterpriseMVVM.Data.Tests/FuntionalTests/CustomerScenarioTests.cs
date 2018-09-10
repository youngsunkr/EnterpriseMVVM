using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseMVVM.Data.Tests.UnitTests
{
    [TestClass]
    public class CustomerScenarioTests : FunctionalTest
    {
        [TestMethod]
        public void AddNewCustomer()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer
                {
                    Email = "customer@northwind.com",
                    FirstName = "David",
                    LastName = "Anderson"
                };
                bc.CreateCustomer(customer);

                bool exists = bc.DataContext.Customers.Any(c => c.Id == customer.Id);

                Assert.IsTrue(exists);
            }
        }


        [TestMethod]
        public void UpdateCustomer_AppliedValuesAreStoredInDataStore()
        {
            using (var bc = new BusinessContext())
            {
                // Arrange
                var customer = new Customer
                {
                    Email = "customer@northwind.com",
                    FirstName = "David",
                    LastName = "Anderson"
                };

                bc.CreateCustomer(customer);
                
                const string newEmail = "new_customer@northwind.com",
                             newFirstName = "Youngsun",
                             newLastName = "Noh";

                customer.Email = newEmail;
                customer.FirstName = newFirstName;
                customer.LastName = newLastName;

                // Act
                bc.UpdateCustomer(customer);

                // Assert
                bc.DataContext.Entry(customer).Reload();

                Assert.AreEqual(newEmail, customer.Email);
                Assert.AreEqual(newFirstName, customer.FirstName);
                Assert.AreEqual(newLastName, customer.LastName);
            }
        }

        [TestMethod]
        public void GetCustomerList_ReturnsExpectedListOfCustomerEntities()
        {
            using (var bc = new BusinessContext())
            {
                bc.CreateCustomer(new Customer { Email = "1@1.com", FirstName = "1", LastName = "a" });
                bc.CreateCustomer(new Customer { Email = "2@2.com", FirstName = "2", LastName = "b" });
                bc.CreateCustomer(new Customer { Email = "3@3.com", FirstName = "3", LastName = "c" });

                var customers = bc.GetCustomerList();

                Assert.IsTrue(customers.ElementAt(0).Id == 1);
                Assert.IsTrue(customers.ElementAt(1).Id == 2);
                Assert.IsTrue(customers.ElementAt(2).Id == 3);
            }
        }

        [TestMethod]
        public void DeleteCustomer_RemovesCustomerFirstDataStore()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer {Email = "1@1.com", FirstName = "1", LastName = "a"};
                bc.CreateCustomer(customer);
                bc.DeleteCustomer(customer);
                Assert.IsFalse(bc.DataContext.Customers.Any());
            }
        }
    }
}
