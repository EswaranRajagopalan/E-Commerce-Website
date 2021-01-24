using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Models
{
    public class Purchase
    {
        public DateTime datePurchased { get; set; }

        public string productName { get; set; }

        public string productID { get; set; }

        public string activationCode { get; set; }

        public string productDesc { get; set; }

        public string image { get; set; }
    }
}
