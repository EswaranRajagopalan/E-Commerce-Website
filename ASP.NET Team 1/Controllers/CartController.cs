using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ASP.NET_Team_1.Models;
using ASP.NET_Team_1.Data;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ASP.NET_Team_1.Controllers
{
    public class CartController : Controller
    {

        /*
         * To display items in the cart
         */
        [HttpGet]
        public IActionResult Index()
        {
            //Retrieve userId and SessionId from cookies
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            string userId = null;
            if (sessionId != null)
            {
                if (HttpContext.Request.Cookies["userId"] != null)
                {
                    userId = HttpContext.Request.Cookies["userId"];
                }

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
             * Retrieve total number of items in the cart, details of items in the cart,sum of items
             * and assign it to ViewData to access from the view
             */
            int totalItems = CartData.GetTotalItems(sess);
            ViewData["TotalItems"] = totalItems;
            List<ProductInCart> cart = CartData.GetAllProducts(sess);
            ViewData["Cart"] = cart;
            double sum = CartData.GetCartSum(sess);

            ViewData["sessionId"] = sessionId;
            ViewData["UserId"] = userId;

            //If no items are present in the cart, redirect to gallery page
            if (sum == 0)
            {
                return RedirectToAction("Index", "Gallery");
            }

            //Redirect to cart Page
            else
            {
                ViewData["Sum"] = sum;
                return View();
            }

        }

        [HttpGet]
        public IActionResult Refresh()
        {
            return RedirectToAction("Index");
        }


        //Action to handle when items in the cart are edited 
        [HttpPost]
        public IActionResult EditProductQuantity([FromBody] ProductInCart product)
        {
            //Retrieve userId and SessionId from cookies
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            string userId = null;
            if (sessionId != null)
            {
                if (HttpContext.Request.Cookies["userId"] != null)
                {
                    userId = HttpContext.Request.Cookies["userId"];
                }

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
            //If there are zero items of a certain product, remove it from the cart
            if (product.Quantity == 0)
            {
                CartData.DeleteFromCart(product, sess);
            }
            //Else update the quantity in the database
            else
            {
                CartData.EditItemQuantity(product, sess);
            }

            return Json(new
            {
                success = true
            });
        }

        //Action to handle when items in the cart are deleted 
        [HttpDelete]
        public IActionResult RemoveFromCart([FromBody] ProductInCart product)
        {
            //Retrieve userId and SessionId from cookies
            string sessionId = HttpContext.Request.Cookies["sessionId"];
            string userId = null;
            if (sessionId != null)
            {
                if (HttpContext.Request.Cookies["userId"] != null)
                {
                    userId = HttpContext.Request.Cookies["userId"];
                }

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

            CartData.DeleteFromCart(product, sess);

            return Json(new
            {
                success = true
            });
        }


        /*
         *Add item to cart from the gallery page 
         */
        [HttpPost]
        public IActionResult GalleryIndex([FromBody] CartItem cartItem)
        {
            //Call function to update the quantity in the database
            CartData.UpdateCart(cartItem);

            return Json(new
            {
                success = true
            });
        }
    }
}
