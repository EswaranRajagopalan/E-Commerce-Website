using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ASP.NET_Team_1.Models;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Team_1.Data;
using Microsoft.AspNetCore.Http;

namespace ASP.NET_Team_1.Controllers
{
    public class LoginController : Controller
    {

        public IActionResult Index(String source)
        {
            ViewData["Is_Login"] = "menu_hilite";
            ViewData["source"] = source;
            return View();
        }

        [HttpPost]
        public IActionResult Authentication(string userId, string password, String source)
        {
            User userAuthen = LoginData.getUserAuthentication(userId, password);

            if (userAuthen!= null)
            {
                string sessionId = HttpContext.Request.Cookies["sessionId"];

                SessionData.updateSession(sessionId, userAuthen);

                HttpContext.Response.Cookies.Append("userId", userAuthen.UserID);
                if (source == "FromCart")
                {
                    return RedirectToAction("Index", "checkOut");
                    
                }
                else
                {
                    return RedirectToAction("Index", "Gallery");
                }
                  
            }
            else
            {
                ViewData["username"] = userId;

                ViewData["Login"] = "menu_hilite";

                ViewData["errMsg"] = "Please check if you have keyed in the correct username or password";
                
                return View("Index");
            }
        }

    }
}
