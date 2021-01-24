using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Models
{
    public class Rating
    {
        public String UserId
        {
            get; set;
        }

        public String ProductId
        {
            get; set;
        }

        public int Rate
        {
            get; set;
        }

    }
}
