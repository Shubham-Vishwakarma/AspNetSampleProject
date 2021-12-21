using Microsoft.EntityFrameworkCore;

namespace BuildRestApiNetCore.Models
{
    public class ProductContext : DbContext
    {
#pragma warning disable CS8618
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
#pragma warning restore CS8618

        public DbSet<Product> Products { get; set; }


    }
}