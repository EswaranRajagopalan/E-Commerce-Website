using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using ASP.NET_Team_1.Models;
using ASP.NET_Team_1.Data;
using System.Diagnostics;


namespace ASP.NET_Team_1.Middlewares
{
    public class SessionValidator
    {
        public readonly RequestDelegate next;

        public SessionValidator(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
                string sessionID = context.Request.Cookies["sessionId"];

                if(sessionID == null)
                {
                    Session session = new Session()
                    {
                        SessionId = Guid.NewGuid().ToString(),
                    };

                    SessionData.SessionInsert(session);
                    
                    context.Response.Cookies.Append("sessionId", session.SessionId);

            }

            await next(context); //going to next middleware
        }
    }
}
