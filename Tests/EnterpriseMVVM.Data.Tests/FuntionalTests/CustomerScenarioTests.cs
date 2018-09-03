using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseMVVM.Data.Tests.UnitTests
{
    [TestClass]
    public class CustomerScenarioTests : FuntionalTest
    {
        [TestMethod]
        public void AddNewCustomer()
        {
            using (var bc = new BusinessContext())
            {
                Customer entity = bc.AddNewCustomer("David", "Anderson");

                bool exists = bc.DataContext.Customers.Any(c => c.Id == entity.Id);

                Assert.IsTrue(exists);
            }
        }
    }
}
