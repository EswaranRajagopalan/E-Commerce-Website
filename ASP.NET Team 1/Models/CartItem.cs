using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Models
{
    public class CartItem
    {
        public String UserId
        {
            get;set;
        }

        public String ProductId
        {
            get; set;
        }

        public String SessionId
        {
            get; set;
        }

        public int Quantity
        {
            get;
            set;
        } 
    }
}
