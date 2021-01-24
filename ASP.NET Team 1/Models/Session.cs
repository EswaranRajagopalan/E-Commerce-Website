using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Models
{
    public class Session
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
 
        public long Timestamp { get; set; }
    }
}
