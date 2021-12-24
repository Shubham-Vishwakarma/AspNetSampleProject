using System;
using System.Collections.Generic;

namespace BuildRestApiNetCore.Models
{
    public partial class Product
    {
        public Product()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Price { get; set; }
        public int Quantity { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
