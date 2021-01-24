using ASP.NET_Team_1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Data
{
    public class CheckoutData : Data
    {
        //clear items in the cart
        public static void CleanCart(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"DELETE FROM Cart where UserID=@uid;";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@uid", userId);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        //Insert Orders to the order records
        public static void UpdateSales(string userId)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"INSERT INTO [Order] (OrderID, [Date Purchased], Username) VALUES (@salesid, @datesales, @username)"; //ordernumber
                SqlParameter param1 = new SqlParameter
                {
                    ParameterName = "@salesid",
                    Value = Guid.NewGuid()
                };
                SqlParameter param2 = new SqlParameter
                {
                    ParameterName = "@datesales",
                    Value = DateTime.Now
                };
                SqlParameter param3 = new SqlParameter
                {
                    ParameterName = "@username",
                    Value = userId
                };
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(param1);
                cmd.Parameters.Add(param2);
                cmd.Parameters.Add(param3);
                cmd.ExecuteNonQuery();
                conn.Close();

            }


        }

        //Get Recent order id from DB
        public static string GetOrderID(string userId)
        {
            string orderID = null;

            using (SqlConnection conn = new SqlConnection(connectionString))

            { 
                conn.Open();

                string sql = @"select Top 1 OrderID from [Order],Cart where Cart.UserId = [Order].Username AND [Order].Username = @username order by [Order].[Date Purchased] DESC";


                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@username", userId);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())

                {

                    orderID = (string)reader["OrderID"];

                }

                conn.Close();

            }
            return orderID;
        }

        //Update sales details in DB
        static public void UpdateSalesDetails(Session sess)
        {
            List<ProductInCart> products = CartData.GetAllProducts(sess);

            string orderID = GetOrderID(sess.UserId);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = null;

                SqlCommand cmd = null;

                for (int i = 0; i < products.Count; i++)
                {
                    for (int j = 0; j < products[i].Quantity; j++)
                    {
                        sql = @"INSERT INTO [Order Details](ProductID, OrderID, [Activation Code]) VALUES ('" + products[i].ProductID + "', '" + orderID + "', '" + Guid.NewGuid() + "')";

                        cmd = new SqlCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }

        }
    }
}
