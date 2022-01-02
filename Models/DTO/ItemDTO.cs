using System;

namespace BuildRestApiNetCore.Models 
{
    public class ItemDTO
    {
        public ItemDTO()
        {
        }

        public ItemDTO(Item item)
        {
            ItemId = item.ItemId;
            ProductId = item.ProductId;
            ProductPrice = item.ProductPrice;
            ProductQuantity = item.ProductQuantity;
            ProductName = item.Product.Name;
            ProductCategory = item.Product.Category;
        }

        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public int ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
    }
}