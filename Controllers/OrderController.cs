#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildRestApiNetCore.Models;
using Microsoft.AspNetCore.Authorization;
using BuildRestApiNetCore.Services.Orders;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrderController(IOrderService service)
        {
            _service = service;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            try
            {
                var user = HttpContext.Items["User"] as AuthenticateResponse;
                var orders = await _service.GetOrders(user.Id);
                return Ok(orders);
            }
            catch
            {
                throw;
            }
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            try
            {
                var order = await _service.GetOrder(id);
                return Ok(order);
            }
            catch(OrderNotFoundException)
            {
                return NotFound();
            }
        }

        // PUT: api/Order/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            try
            {
                var updatedOrder = await _service.UpdateOrder(order);
                return Ok(updatedOrder);
            }
            catch
            {
                return NoContent();
            }
        }

        // POST: api/Order
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            var createdOrder = await _service.CreateOrder(order);
            return CreatedAtAction(nameof(GetOrder), new { id = createdOrder.OrderId }, createdOrder);
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                await _service.DeleteOrder(id);
                return Ok(new { message = "Delete Successfull" });
            }
            catch(OrderNotFoundException)
            {
                return NoContent();
            }
        }
    }
}
