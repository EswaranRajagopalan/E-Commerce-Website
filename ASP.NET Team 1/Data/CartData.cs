using Microsoft.AspNetCore.Mvc;
using ASP.NET_Team_1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
using System.Diagnostics;


namespace ASP.NET_Team_1.Data
{
    public class CartData : Data
    {
        // Add Items to the cart
        public static void UpdateCart(CartItem cartItem)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                /*Check if item is already present in the cart
                 * Returns -1 if item is not present in the table
                 */
                int quantity = GetQuantity(cartItem);
                String sql = "";
                SqlCommand cmd = new SqlCommand();

                //if item is already in the database table
                if (quantity != -1)
                {
                    quantity = quantity + cartItem.Quantity;//increment quantity

                    /*UserId value is checked to see if customer is logged in
                     * If customer is not logged in, update temporary cart using session id
                     * If customer is logged in, update cart using user id
                     */
                    if (cartItem.UserId != null)//logged in
                    {
                        sql = @"UPDATE Cart SET Quantity=@Quantity WHERE UserId=@UserId AND ProductID=@ProductId";
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@UserId", cartItem.UserId);
                    }
                    else //not logged in
                    {
                        sql = @"UPDATE TemporaryCart SET Quantity=@Quantity WHERE SessionId=@SessionId AND ProductID=@ProductId";
                        cmd = new SqlCommand(sql, conn);
                    }
                }
                else // item not present in cart 
                {
                    if (cartItem.UserId != null) //user logged in
                    {
                        sql = @"INSERT INTO Cart VALUES(@UserId,@ProductId,@Quantity)";
                        cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@UserId", cartItem.UserId);
                    }
                    else // user not logged in
                    {
                        sql = @"INSERT INTO TemporaryCart VALUES(@SessionId,@ProductId,@Quantity)";
                        cmd = new SqlCommand(sql, conn);
                    }

                    quantity = cartItem.Quantity;
                }
                cmd.Parameters.AddWithValue("@ProductId", cartItem.ProductId);
                cmd.Parameters.AddWithValue("@SessionId", cartItem.SessionId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);

                int affected = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /*Checks if item is already present in the cart
         * Returns -1 if item is not present
         * Returns quantity if item is present
         */
        public static int GetQuantity(CartItem cart)
        {
            int quantity = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();
                if (cart.UserId == null)
                {
                    sql = @"SELECT Quantity FROM TemporaryCart WHERE ProductID=@ProductId AND SessionId=@SessionId";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                    cmd.Parameters.AddWithValue("@SessionId", cart.SessionId);
                }
                else
                {
                    sql = @"SELECT Quantity FROM Cart WHERE ProductID=@ProductId AND UserId=@UserId";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ProductId", cart.ProductId);
                    cmd.Parameters.AddWithValue("@UserId", cart.UserId);
                }
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    quantity = (int)reader["Quantity"];
                }
                conn.Close();
            }

            return quantity;
        }

        //Move items from temporary cart to  cart
        public static void MoveToCart(string sessionId, string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                String sql = @"SELECT ProductID,Quantity
                                FROM TemporaryCart WHERE SessionId=@SessionId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                SqlDataReader reader = cmd.ExecuteReader();
                List<CartItem> list = new List<CartItem>();
                while (reader.Read())
                {
                    string pId = (string)reader["ProductID"];
                    int quantity = (int)reader["Quantity"];
                    CartItem cItem = new CartItem()
                    {
                        UserId = userId,
                        ProductId = pId,
                        Quantity = quantity,
                        SessionId = sessionId
                    };
                    list.Add(cItem);
                }
                conn.Close();

                /*Call updatecart function to move each item from temporary cart to cart
                 * after user logs in
                 */
                foreach (CartItem ci in list)
                {
                    UpdateCart(ci);
                }

            }
        }

        //Delete items from temporary Cart
        public static void ClearTemporaryCart(string sessionId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                String sql = @"DELETE FROM TemporaryCart WHERE SessionId=@SessionId";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@SessionId", sessionId);
                Debug.WriteLine(sql);
                int affected = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        //Get total number of items in the cart
        public static int GetTotalItems(Session sess)
        {
            int total = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();
                if (sess.UserId != null)//user is logged in
                {
                    sql = @"SELECT SUM(Quantity) FROM Cart WHERE UserId=@UserId";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@UserId", sess.UserId);
                }
                else //user is not logged in
                {
                    sql = @"SELECT SUM(Quantity) FROM TemporaryCart WHERE SessionId=@SessionId";
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@SessionId", sess.SessionId);
                }
                object o = cmd.ExecuteScalar();

                //return 0 if no items are present in the DB
                total = cmd.ExecuteScalar() == DBNull.Value ? 0 : (int)cmd.ExecuteScalar();
            }

            return total;
        }

        //Retrieve items in the cart
        public static List<ProductInCart> GetAllProducts(Session sess)
        {
            List<ProductInCart> cart = new List<ProductInCart>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();
                if (sess.UserId != null) //user is logged in
                {
                    sql = @"SELECT Product.ProductName as pName , Product.Description as description,Product.[Unit Price] as price,Cart.Quantity as quantity,Product.ProductLogo as logo,Product.ProductID as productID
                                FROM Cart,Product where Cart.ProductID=Product.ProductID and Cart.UserId='" + sess.UserId + "'";
                }
                else //user is not logged in
                {
                    sql = @"SELECT Product.ProductName as pName , Product.Description as description,Product.[Unit Price] as price,TemporaryCart.Quantity as quantity,Product.ProductLogo as logo,Product.ProductID as productID
                                FROM TemporaryCart,Product where TemporaryCart.ProductID=Product.ProductID and TemporaryCart.SessionId='" + sess.SessionId + "'";
                }
                cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ProductInCart product = new ProductInCart()
                    {
                        ProductName = (string)reader["pName"],
                        Description = (string)reader["description"],
                        ProductLogo = (string)reader["logo"],
                        Price = Convert.ToDouble(reader["price"]),
                        Quantity = (int)reader["quantity"],
                        ProductID = (string)reader["ProductID"]
                    };
                    cart.Add(product);
                }

                conn.Close();
            }

            return cart;
        }

        // Need to get the total sum of items in the cart
        public static double GetCartSum(Session sess)
        {
            Double sum = 0.0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();
                if (sess.UserId != null) //user is logged in
                {
                    sql = @"select sum(Product.[Unit Price]*Cart.Quantity) as Sum from Cart,Product 
                where Cart.ProductID=Product.ProductID and Cart.UserId='" + sess.UserId + "'";
                }
                else //user is not logged in
                {
                    sql = @"select sum(Product.[Unit Price]*TemporaryCart.Quantity) as Sum from TemporaryCart,Product 
                where TemporaryCart.ProductID=Product.ProductID and TemporaryCart.SessionId='" + sess.SessionId + "'";
                }

                cmd = new SqlCommand(sql, conn);

                sum = cmd.ExecuteScalar() == DBNull.Value ? 0.0 : Convert.ToDouble(cmd.ExecuteScalar());
            }

            return sum;
        }

        //To update the database when user edits the cart 
        public static void EditItemQuantity(ProductInCart P, Session sess)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                if (sess.UserId != null) // user is logged in
                {
                    sql = @"update Cart set Quantity ='" + P.Quantity + "'where UserId='" + sess.UserId + "' and ProductID= '" + P.ProductID + "'";
                }
                else // user is not logged in
                {
                    sql = @"update TemporaryCart set Quantity ='" + P.Quantity + "'where SessionId='" + sess.SessionId + "' and ProductID= '" + P.ProductID + "'";
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                int noAfffectedRows = cmd.ExecuteNonQuery();
            }

        }

        // To remove an item from the Cart records 
        public static void DeleteFromCart(ProductInCart P, Session sess)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                if (sess.UserId != null) //user is logged in
                {
                    sql = @"DELETE FROM Cart WHERE ProductID ='" + P.ProductID + "' and UserId='" + sess.UserId + "'";
                }
                else // user is not logged in
                {
                    sql = @"DELETE FROM TemporaryCart WHERE ProductID ='" + P.ProductID + "' and SessionId='" + sess.SessionId + "'";
                }
                SqlCommand cmd = new SqlCommand(sql, conn);
                int noAfffectedRows = cmd.ExecuteNonQuery();
            }

        }
    }
}