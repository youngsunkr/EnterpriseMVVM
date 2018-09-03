using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseMVVM.Data
{
    public sealed class BusinessContext : IDisposable
    {
        private readonly DataContext context;
        private bool disposed;

        public BusinessContext()
        {
            context = new DataContext();
        }

        public DataContext DataContext
        {
            get { return context; }
        }

        public Customer AddNewCustomer(string firstName, string lastName)
        {
            if (firstName == null)
                throw new ArgumentException("firstName", "firstName must be non-null");

            if (string.IsNullOrEmpty(firstName))
                throw new ArgumentException("firstName must not be an empty string.", "firstName");

            var customer = new Customer
            {
                FirstName = firstName,
                LastName = lastName
            };

            context.Customers.Add(customer);
            context.SaveChanges();

            return customer;
        }

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed || !disposing)
                return;

            if (context != null)
                context.Dispose();

            disposed = true;
        } 
        #endregion
    }
}
