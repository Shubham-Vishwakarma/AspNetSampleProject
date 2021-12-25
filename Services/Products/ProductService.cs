using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BuildRestApiNetCore.Models;
using BuildRestApiNetCore.Exceptions;

namespace BuildRestApiNetCore.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ShopbridgeContext _context;

        public ProductService(ShopbridgeContext context)
        {
            _context = context;
        }

        public string ServiceName 
        {
            get 
            {
                return "ProductService";
            }
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if(product == null)
                throw new ProductNotFoundException($"No product found with id {id}");

            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                try
                {
                    var p = GetProduct(product.Id);
                }
                catch(ProductNotFoundException ex)
                {
                    throw ex;
                }
                throw;
            }

            return product;
        }

        public async Task DeleteProduct(int id)
        {
            try
            {
                var product = await GetProduct(id);
                await DeleteProduct(product);
            }
            catch(ProductNotFoundException pnfe)
            {
                throw pnfe;
            }
        }

        public async Task DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}