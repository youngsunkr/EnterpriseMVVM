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
    public class MainViewModelTests
    {
        [TestMethod]
        public void IsViewModel()
        {
            Assert.IsTrue(typeof(MainViewModel).BaseType == typeof(ViewModel));
        }

        [TestMethod]
        public void ValidationErrorWhenCustomerNameExceeds32Characters()
        {
            var viewModel = new MainViewModel
            {
                CustomerName = "1234567890abcd efghijklmnopqrstv"
            };

            Assert.IsNotNull(viewModel["CustomerName"]);
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
    }
}
