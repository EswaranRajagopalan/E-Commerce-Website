using ASP.NET_Team_1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using ASP.NET_Team_1.Data;

namespace ASP.NET_Team_1.Data
{
    public class ProductData : Data
    {
        /*
         *Takes the user ID as parameter and returns recommended products based on
         *cart and previous purchase history
         */
        public static List<Product> GetRecommendedProducts(String userId)
        {
            string sql = @"SELECT TOP 3 * FROM Product "+
                         "WHERE ProductCategory IN(SELECT p.ProductCategory FROM Cart c,Product p "+
                         "WHERE p.ProductId=c.ProductId AND c.UserId='" + userId + "') " +
                         "UNION SELECT TOP 3 * FROM Product WHERE ProductId IN(SELECT DISTINCT od.ProductID " +
                         "FROM [ShoppingWebV2].[dbo].[Order] o,[Order Details] od " +
                         "WHERE o.OrderID=od.OrderID AND o.Username='" + userId + "')";

            return RetrieveProducts(sql);
        }

        /*
         * Returns a list of most sold products 
         */
        public static List<Product> GetPopularProducts()
        {
            string sql = @"SELECT *FROM Product
                         WHERE ProductId IN(SELECT TOP 6 ProductID FROM [Order Details] 
                         GROUP BY ProductID 
                         ORDER BY COUNT(*) DESC)";
            return RetrieveProducts(sql);
        }

        /*
         * Returns a list of products with the highest rating
         */
        public static List<Product> GetHighRatedProducts()
        {
            string sql = @"SELECT TOP 6 * FROM Product 
                         ORDER BY Rating DESC";

            return RetrieveProducts(sql);

        }

        /*
         * Returns a list of products based on search criteria
         */
        public static List<Product> GetSearchProducts(string criteria)
        {
            string sql = @"SELECT * FROM Product 
                         WHERE Description LIKE '%" + criteria + "%' " +
                         "OR ProductName LIKE '%" + criteria + "%' ";

            return RetrieveProducts(sql);

        }

        /*
         * Helper function that takes in sql as parameter 
         *and returns product information from the product table
         */
        public static List<Product> RetrieveProducts(string sql)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product product = new Product()
                    {
                        ProductID = (string)reader["ProductID"],
                        ProductName = (string)reader["ProductName"],
                        Description = (string)reader["Description"],
                        Price = Convert.ToDouble(reader["Unit Price"]),
                        ProductCategory = (string)reader["ProductCategory"],
                        ProductLogo = (string)reader["ProductLogo"],
                        NumReviews = (int)reader["NumReviews"],
                        Rating = Convert.ToDouble(reader["Rating"])
                    };
                    products.Add(product);
                }
            }
            return products;
        }
    }
}
