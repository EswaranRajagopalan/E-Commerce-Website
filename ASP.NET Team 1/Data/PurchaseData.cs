using ASP.NET_Team_1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.Design;

namespace ASP.NET_Team_1.Data
{
    public class PurchaseData : Data
    {
        public static List<Purchase> getUserPurchases(string userId)
        {
            List<Purchase> purchases = new List<Purchase>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"select o.[Date Purchased], od.ProductID, p.[ProductName], od.[Activation Code], p.[Description], p.[ProductLogo]
                                from [Order Details] od, [Order] o, [Product] p, [Session] s
                                where o.OrderID = od.OrderID AND od.ProductID = p.ProductID AND o.Username = s.UserId
                                AND s.userId = '"+ userId +"'";

                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    purchases.Add(new Purchase()
                    {
                        datePurchased = (DateTime) reader["Date Purchased"],
                        productID = (string) reader["ProductID"],
                        productName = (string) reader["ProductName"],
                        activationCode = reader["Activation Code"].ToString(),
                        productDesc = (string) reader["Description"],
                        image = (string) reader["ProductLogo"]
                    });

                }

                conn.Close();

                return purchases;
            }
        }

    }
}
