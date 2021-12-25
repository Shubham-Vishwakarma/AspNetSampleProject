using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildRestApiNetCore.Models;

namespace BuildRestApiNetCore.Services.Customers
{
    public interface ICustomerService : IService
    {
        Task<IEnumerable<Customer>> GetCustomers();
        Task<Customer> GetCustomer(int id);
        Task<Customer> GetCustomer(string email, string password);
        Task<Customer> CreateCustomer(Customer customer);
        Task<Customer> UpdateCustomer(Customer customer);
        Task DeleteCustomer(int id);
        Task DeleteCustomer(Customer customer);
    }
}