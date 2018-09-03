using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseMVVM.Data.Tests.UnitTests
{
    [TestClass]
    public class BusinessContextTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewCustomer_ThrowException_WhenFirstNameIsNull()
        {
            using (var bc = new BusinessContext())
            {
                bc.AddNewCustomer(null, "Anderson");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNewCustomer_ThrowException_WhenFirstNameEmpty()
        {
            using (var bc = new BusinessContext())
            {
                bc.AddNewCustomer("", "Anderson");
            }
        }

    }
}
