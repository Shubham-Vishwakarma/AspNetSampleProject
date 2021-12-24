using System;
using System.Collections.Generic;

namespace BuildRestApiNetCore.Models
{
    public partial class Order
    {
        public Order()
        {
            Items = new HashSet<Item>();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<Item> Items { get; set; }
    }
}
