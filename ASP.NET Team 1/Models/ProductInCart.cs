using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Models
{
    public class ProductInCart:Product
    {
        public int Quantity { get; set; }
        public double Total { get; set; }
    }
}
