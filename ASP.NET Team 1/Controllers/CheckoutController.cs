using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP.NET_Team_1.Data;
using ASP.NET_Team_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.NET_Team_1.Controllers
{
    public class checkoutController : Controller
    {
        /*
         * To display summary of items before payment
         */
        public IActionResult Index()
        {
            //Retrieve userId and SessionId from cookies
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            string userId = HttpContext.Request.Cookies["userId"];

            /*
             *Session object allows to query based on user id 
             *Although user id is enough, creating a session object allows code reusability
             */
            Session sess = new Session()
            {
                SessionId = sessionId,
                UserId = userId
            };

            /*Retrieve products from the cart and total number and price of items 
             * and Assign to view Data to access from views
             */
            List<ProductInCart> product = CartData.GetAllProducts(sess);
            double sum = CartData.GetCartSum(sess);
            ViewData["product"] = product;
            ViewData["total"] = sum;
            int totalItems = CartData.GetTotalItems(sess);
            ViewData["TotalItems"] = totalItems;

            ViewData["sessionId"] = sessionId;
            ViewData["UserId"] = userId;
            return View();
        }

        //Clean items from the cart and update order and order details
        public IActionResult CleanAndUpdate()
        {
            //Retrieve SessionId and UserId From Cookies
            string sessionId = Request.Cookies["sessionId"];
            string userId = Request.Cookies["userId"];

            Session sess = new Session()
            {
                SessionId = sessionId,
                UserId = userId
            };
            //Call functions to update sales records and clear items from the cart
            CheckoutData.UpdateSales(userId);
            CheckoutData.UpdateSalesDetails(sess);
            CheckoutData.CleanCart(userId);

            //Redirect to gallery page after database update
            return RedirectToAction("Index", "Gallery");
        }
    }
}

