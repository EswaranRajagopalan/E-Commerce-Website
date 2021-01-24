using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Models
{
    public class Product
    {
        public string ProductID { get; set; }
        public string ProductLogo { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public double Rating
        {
            get;
            set;
        }

        public int NumReviews{
            get;
            set;
        }
        public int subtotal { get; set; }

    }
}
