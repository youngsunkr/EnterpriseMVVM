using EnterpriseMVVM.Data;
using EnterpriseMVVM.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseMVVM.DesktopClient.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private IBusinessContext context;
        private string customerName;
        private Customer selectedCustomer;

        public MainViewModel(IBusinessContext context)
        {
            Customers = new ObservableCollection<Customer>();
            this.context = context;
        }

        public bool CanModify
        {
            get
            {
                return SelectedCustomer != null;
            }
        }

        [Required]
        [StringLength(32, MinimumLength = 2)]
        public string CustomerName
        {
            get{ return customerName; }
            set
            {
                customerName = value;
                NotifyPropertyChanged();
            }
        }

        public ICollection<Customer> Customers { get; private set; }
        public ICommand GetCustomerListCommand
        {
            get
            {
                return new ActionCommand(p => GetCustomerList());
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set
            {
                selectedCustomer = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged("CanModify");
            }
        }

        public bool IsValid
        {
            get
            {
                return SelectedCustomer == null ||
                    !String.IsNullOrWhiteSpace(SelectedCustomer.FirstName) &&
                    !String.IsNullOrWhiteSpace(SelectedCustomer.LastName) &&
                    !String.IsNullOrWhiteSpace(SelectedCustomer.Email);
            }
        }
        public ICommand AddCommand
        {
            get
            {
                return new ActionCommand(p => AddCustomer(),
                                         p => IsValid);
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new ActionCommand(p => DeleteCustomer());
            }
        }

        public ICommand UpdateCommand
        {
            get
            {
                return new ActionCommand(p => SaveCustomer(),
                                        p => IsValid);
            }
        }

        public void AddCustomer()
        {
            var customer = new Customer
            {
                FirstName = "New",
                LastName = "Customer",
                Email = "new@customer.com"
            };
        
            try
            {
                context.CreateCustomer(customer);
            }
            catch (Exception ex)
            {
                // TODO: In later session, cover error handling
                return;
            }
        
            Customers.Add(customer);
        }


        private void GetCustomerList()
        {
            Customers.Clear();
            //SelectdCustomer = null;

            foreach (var customer in context.GetCustomerList())
                Customers.Add(customer);
            

        }

        private void SaveCustomer()
        {
            context.UpdateCustomer(SelectedCustomer);
        }
        private void DeleteCustomer()
        {
            context.DeleteCustomer(SelectedCustomer);
            Customers.Remove(SelectedCustomer);
            SelectedCustomer = null;
        }
    }
}
