using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Team_1.Data;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Team_1.Models;
using Microsoft.AspNetCore.Http;

namespace ASP.NET_Team_1.Controllers
{
    public class MyPurchasesController : Controller
    {
        public IActionResult Index()
        {
            string sessionID = HttpContext.Request.Cookies["sessionId"];
            string userID = HttpContext.Request.Cookies["userId"];

            List<Purchase> purchases = PurchaseData.getUserPurchases(userID);

            ViewData["Purchases"] = purchases;

            ViewData["UserId"] = userID;

            ViewData["sessionId"] = sessionID;
            Session sess = new Session()
            {
                SessionId = sessionID,
                UserId = userID
            };
            int totalItems = CartData.GetTotalItems(sess);
            ViewData["TotalItems"] = totalItems;
            return View();
        }

        [HttpPost]
        public IActionResult RateProduct([FromBody] Rating pRating)
        {
            //sadsadasdadsa
            RatingData.updateRating(pRating);

            return Json(new
            {
                success = true
            });
        }
    }
}
