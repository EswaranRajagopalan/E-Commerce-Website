using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Team_1.Data;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Team_1.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {

            string sessionId = HttpContext.Request.Cookies["sessionId"];

            SessionData.deleteSession(sessionId);

            HttpContext.Response.Cookies.Delete("sessionId");
            HttpContext.Response.Cookies.Delete("userId");
            HttpContext.Session.Clear();

            return RedirectToAction("Index","Gallery");
        }
    }
}
