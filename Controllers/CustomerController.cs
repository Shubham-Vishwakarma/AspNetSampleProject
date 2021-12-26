#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Exceptions;
using BuildRestApiNetCore.Services.Customers;
using Microsoft.AspNetCore.Authorization;

namespace BuildRestApiNetCore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;

        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        // GET: api/Customer
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await _service.GetCustomers();
            return Ok(customers);
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
            {
                var customer = await _service.GetCustomer(id);
                return Ok(customer);
            }
            catch(CustomerNotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Customer>> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            try
            {
                var updatedCustomer = await _service.UpdateCustomer(customer);
                return Ok(updatedCustomer);
            }
            catch(CustomerNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var createdCustomer = await _service.CreateCustomer(customer);
            return CreatedAtAction(nameof(GetCustomer), new { id = createdCustomer.Id }, createdCustomer);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseMessage>> DeleteCustomer(int id)
        {
            try
            {
                await _service.DeleteCustomer(id);
                return Ok(new ResponseMessage("Delete Successfull"));
            }
            catch(CustomerNotFoundException)
            {
                return NotFound();
            }
        }

    }
}
