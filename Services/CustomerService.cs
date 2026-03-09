using NordFlow.Models;

namespace NordFlow.Services;

public class CustomerService
{
    public List<Customer> GetCustomers()
    {
        return new List<Customer>
        {
            new Customer { Id = 1, Name = "Anna Svensson", Email = "anna@gmail.com" },
            new Customer { Id = 2, Name = "Erik Johansson", Email = "erik@gmail.com" },
            new Customer { Id = 3, Name = "Sara Ali", Email = "sara@gmail.com" }
        };
    }

    public IEnumerable<Customer> Customers { get; set; }
}