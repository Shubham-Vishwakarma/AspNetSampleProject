using System;

namespace BuildRestApiNetCore.Models.DTO
{
    public partial class ProductDTO
    {
        public ProductDTO()
        {
        }

        public ProductDTO(Product product)
        {
            Id = product.Id;
            Name = product.Name;
            Category = product.Category;
            Price = product.Price;
            Quantity = product.Quantity;
            Image = product.Image;
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; } = null!;

    }
}
