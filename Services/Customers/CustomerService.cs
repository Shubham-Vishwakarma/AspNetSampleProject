using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ShopbridgeContext _context;

        public CustomerService(ShopbridgeContext context)
        {
            _context = context;
        }

        public string ServiceName
        {
            get
            {
                return "CustomerService";
            }
        }

        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> GetCustomer(int id)
        {
            var c = _context.Customers;
            var customer = await _context.Customers.FindAsync(id);
            
            if(customer == null)
            {
                throw new CustomerNotFoundException($"No Customer found will id = {id}");
            }

            return customer;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                try
                {
                    var c = await GetCustomer(customer.Id);
                }
                catch(CustomerNotFoundException ex)
                {
                    throw ex;
                }
                throw;
            }
            
            return customer;
        }

        public async Task DeleteCustomer(int id)
        {
            try
            {
                var customer = await GetCustomer(id);
                await DeleteCustomer(customer);
            }
            catch(CustomerNotFoundException ex)
            {
                throw ex;
            }
        }

        public async Task DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }


        public async Task<Customer> GetCustomer(string email, string password)
        {
            var customer = await _context.Customers
                                .Where(p => p.Email.Equals(email) && p.Password.Equals(password))
                                .FirstOrDefaultAsync();

            if(customer == null)
                throw new CustomerNotFoundException("No customer can be found with the entered credentials");

            return customer;
        }

    }
}