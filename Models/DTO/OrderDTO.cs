using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildRestApiNetCore.Models
{
    public class OrderDTO
    {
        public OrderDTO()
        {
            Items = new HashSet<ItemDTO>();
        }

        public OrderDTO(Order order) 
        {
            OrderId = order.OrderId;
            OrderDate = order.OrderDate;
            Items = order.Items
                        .Select(item => new ItemDTO(item))
                        .ToHashSet();
        }

        public int OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
        public ICollection<ItemDTO> Items { get; set; }
    }
}