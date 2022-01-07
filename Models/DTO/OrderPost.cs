using System;
using System.Collections.Generic;

namespace BuildRestApiNetCore.Models 
{
    public class OrderPost
    {
        public OrderPost()
        {
            Items = new HashSet<ItemPost>();
        }

        public int CustomerId { get; set; }
        public ICollection<ItemPost> Items { get; set; }
    }

    public class ItemPost
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductPrice { get; set; }
    }
}