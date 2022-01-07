using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ShopbridgeContext _context;

        public OrderService(ShopbridgeContext context)
        {
            _context = context;
        }

        public string ServiceName 
        {
            get 
            {
                return "OrderService";
            }
        }

        public async Task<IEnumerable<Order>> GetOrders(int customerId)
        {
            var orders = await _context.Orders
                            .Where(o => o.CustomerId == customerId)
                            .Include(o => o.Items)
                            .ThenInclude(i => i.Product)
                            .ToListAsync();

            return orders;
        }

        public async Task<IEnumerable<OrderDTO>> GetOrderDTOs(int customerId)
        {
            var orders = await _context.Orders
                            .Where(o => o.CustomerId == customerId)
                            .Include(o => o.Items)
                            .ThenInclude(i => i.Product)
                            .Select(o => new OrderDTO(o))
                            .ToListAsync();

            return orders;
        }

        public async Task<Order> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if(order == null)
                throw new OrderNotFoundException($"No Order can be found with id {id}");

            await _context.Entry(order)
                .Collection(o => o.Items)
                .Query()
                .Include(i => i.Product)
                .LoadAsync();

            await _context.Entry(order)
                .Reference(o => o.Customer)
                .LoadAsync();

            return order;
        }
        
        public async Task<Order> CreateOrder(OrderPost orderPost)
        {
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.CustomerId = orderPost.CustomerId;
            foreach(ItemPost itemPost in orderPost.Items)
            {
                Item item = new Item();
                item.ProductId = itemPost.ProductId;
                item.ProductQuantity = itemPost.ProductQuantity;
                item.ProductPrice = itemPost.ProductPrice;
                order.Items.Add(item);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order> UpdateOrder(Order order)
        {
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return order;
            }
            catch (DbUpdateConcurrencyException)
            {
                try
                {
                    var o = await GetOrder(order.OrderId);
                }
                catch(OrderNotFoundException oex)
                {
                    throw oex;
                }
                throw;
            }
        }

        public async Task DeleteOrder(int id)
        {
            try
            {
                var order = await GetOrder(id);
                await DeleteOrder(order);
            }
            catch(OrderNotFoundException ex)
            {
                throw ex;
            }
        }

        public async Task DeleteOrder(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }
}