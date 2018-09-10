using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseMVVM.Data.Tests.UnitTests
{
    [TestClass]
    public class BusinessContextTests : FunctionalTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewCustomer_ThrowException_WhenEmailNameIsEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer
                {
                    Email = "",
                    FirstName = "David",
                    LastName = "Anderson"
                };
                bc.CreateCustomer(customer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewCustomer_ThrowException_WhenFirstNameIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer
                {
                    Email = "customer@northwind.com",
                    FirstName = null,
                    LastName = "Anderson"
                };
                bc.CreateCustomer(customer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewCustomer_ThrowException_WhenFirstNameEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer
                {
                    Email = "customer@northwind.com",
                    FirstName = "",
                    LastName = "Anderson"
                };
                bc.CreateCustomer(customer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNewCustomer_ThrowException_WhenLastNameIsNull()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer
                {
                    Email = "customer@northwind.com",
                    FirstName = "David",
                    LastName = null
                };
                bc.CreateCustomer(customer);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewCustomer_ThrowException_WhenLastNameEmpty()
        {
            using (var bc = new BusinessContext())
            {
                var customer = new Customer
                {
                    Email = "customer@northwind.com",
                    FirstName = "David",
                    LastName = ""
                };
                bc.CreateCustomer(customer);
            }
        }
    }
}
