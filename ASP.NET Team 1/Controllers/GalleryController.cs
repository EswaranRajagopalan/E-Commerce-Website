using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Team_1.Models;
using ASP.NET_Team_1.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ShoppingWebApp.Controllers
{
    public class GalleryController : Controller
    {
        /*
         * Index action that acts as the homepage and displays the list of products
         */
        public IActionResult Index()
        {
            /*
             * Retrieve popular products and products with high rating from the database 
             * And assign it to view data to access from the view
             */
            List<Product> popularProducts = ProductData.GetPopularProducts();
            List<Product> highRatedProducts = ProductData.GetHighRatedProducts();
            ViewData["highRatedProducts"] = highRatedProducts;
            ViewData["popularProducts"] = popularProducts;

            //Retrive sessionID and UserId(if exists) from cookies
            string userId = null;
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            if (sessionId != null)
            {
                if (HttpContext.Request.Cookies["userId"] != null)
                {
                    userId = HttpContext.Request.Cookies["userId"];
                    /*
                     * View recommended products only if the user
                     * is logged in.
                     * Retrieve recommended products from the database and
                     * assign it to view data to access from the view
                     */
                    List<Product> recommendedProducts = ProductData.GetRecommendedProducts(userId);
                    ViewData["recommendedProducts"] = recommendedProducts;

                }

                /*
                 * Create a session object to query data since items in the cart can be temporary
                 * or permanent based on whether the user has logged in or not.
                 * Session object allows to query based on session id or user id based on whether the user
                 * has logged in
                 */

                Session sess = new Session()
                {
                    SessionId = sessionId,
                    UserId = userId
                };

                /*
                 * Retrieve total number of items in the cart and assign it to ViewData
                 * to access from the view
                 */
                int totalItems = CartData.GetTotalItems(sess);
                ViewData["TotalItems"] = totalItems;
            }

            /*
             * Pass the sessionId and UserId as ViewData to access from the views 
             */
            ViewData["sessionId"] = sessionId;
            ViewData["UserId"] = userId;

            return View();
        }

        //Search action to search for products
        public IActionResult Search(String criteria)
        {
            /*
             * Retrieve products matching the search criteria from the database 
             * And assign it to view data to access from the view
             */
            List<Product> products = ProductData.GetSearchProducts(criteria);
            ViewData["products"] = products;

            //Retrive sessionID and UserId(if exists) from cookies
            string userId = null;
            string sessionId = HttpContext.Request.Cookies["sessionId"];

            if (sessionId != null)
            {
                if (HttpContext.Request.Cookies["userId"] != null)
                {
                    userId = HttpContext.Request.Cookies["userId"];
                }

                /*
                 * Create a session object to query data since items in the cart can be temporary
                 * or permanent based on whether the user has logged in or not.
                 * Session object allows to query based on session id or user id based on whether the user
                 * has logged in
                 */
                Session sess = new Session()
                {
                    SessionId = sessionId,
                    UserId = userId
                };

                /*
                 * Retrieve total number of items in the cart and assign it to ViewData
                 * to access from the view
                 */
                int totalItems = CartData.GetTotalItems(sess);
                ViewData["TotalItems"] = totalItems;
            }

            /*
             * Pass the sessionId and UserId as ViewData to access from the views 
             */
            ViewData["sessionId"] = sessionId;
            ViewData["UserId"] = userId;
            return View();
        }
    }
}
