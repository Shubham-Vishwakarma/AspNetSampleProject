using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BuildRestApiNetCore.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        [JsonIgnore]
        public string Password { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
