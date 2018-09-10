using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseMVVM.Data
{
    public sealed class BusinessContext : IDisposable, IBusinessContext
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

        public void CreateCustomer(Customer customer)
        {
            Check.Require(customer.Email);
            Check.Require(customer.FirstName);
            Check.Require(customer.LastName);
           
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            var entity = context.Customers.Find(customer.Id);

            if (entity == null)
            {
                throw new NotImplementedException("Handle appropriately for your API design.");
            }

            context.Entry(customer).CurrentValues.SetValues(customer);
            context.SaveChanges();
        }

        public ICollection<Customer> GetCustomerList()
        {
            return context.Customers.OrderBy(p => p.Id).ToArray();
            //return context.Customers.OrderBy(p => p.Id).ToList();
        }
        static class Check
        {
            public static void Require(string value)
            {
                if (value == null)
                    throw new ArgumentNullException();
                else if (value.Trim().Length == 0)
                    throw new ArgumentException();

            }
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
