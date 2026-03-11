// ========================================
// CUSTOMER SERVICE - In-memory (ingen DB än)
// ========================================
using NordFlow.Models;
using System.Collections.ObjectModel;

namespace NordFlow.Services
{
    public class CustomerService
    {
        public ObservableCollection<Customer> Customers { get; } = new();

        public void AddCustomer(Customer customer)
        {
            customer.Id = Customers.Count + 1;
            Customers.Add(customer);
        }

        public void RemoveCustomer(Customer customer)
        {
            Customers.Remove(customer);
        }
    }
}