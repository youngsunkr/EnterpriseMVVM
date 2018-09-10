using System.Collections.Generic;

namespace EnterpriseMVVM.Data
{
    public interface IBusinessContext
    {
        void CreateCustomer(Customer customer);

        void DeleteCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        ICollection<Customer> GetCustomerList();
    }
}