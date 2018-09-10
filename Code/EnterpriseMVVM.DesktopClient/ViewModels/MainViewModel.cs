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
        private BusinessContext context;
        private string customerName;
        private Customer selectedCustomer;

        public MainViewModel() : this(new BusinessContext())
        {

        }

        public MainViewModel(BusinessContext context)
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

        public ICommand DeleteCustomerCommand
        {
            get
            {
                return new ActionCommand(p => DeleteCustomer());
            }
        }

        public ICommand SaveCustomerCommand
        {
            get
            {
                return new ActionCommand(p => SaveCustomer(),
                                        p => IsValid);
            }
        }

        public void AddCustomer()
        {
            using (var api = new BusinessContext())
            {
                var customer = new Customer
                {
                    FirstName = "New",
                    LastName = "Customer",
                    Email = "new@customer.com"
                };
            
                try
                {
                    api.AddNewCustomer(customer);
                }
                catch (Exception ex)
                {
                    // TODO: In later session, cover error handling
                    return;
                }
            
                Customers.Add(customer);
            }
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
            context.DataContext.Customers.Remove(SelectedCustomer);
            context.DataContext.SaveChanges();
            Customers.Remove(SelectedCustomer);
            SelectedCustomer = null;
        }
    }
}
