using ASP.NET_Team_1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Data
{
    public class RatingData : Data
    {
        //Update rating of item in DB
        public static void updateRating(Rating rate)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                int rating = GetRating(rate);
                String sql = "";
                SqlCommand cmd = new SqlCommand();
                if (rating != -1)
                {
                    sql = @"UPDATE ProductRating SET Rating=@rating WHERE UserId=@UserId AND ProductID=@ProductId";

                }
                else
                {
                    sql = @"INSERT INTO ProductRating VALUES(@UserId,@ProductId,@rating)";

                }

                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", rate.ProductId);
                cmd.Parameters.AddWithValue("@UserId", rate.UserId);
                cmd.Parameters.AddWithValue("@rating", rate.Rate);

                int affected = cmd.ExecuteNonQuery();
                conn.Close();
            }

            //Update average rating in product DB
            UpdateProductDB(rate.ProductId);
            
        }

        //Checks if rating for an item by an user is already present in the DB
        public static int GetRating(Rating rate)
        {
            int rating = -1;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();

                sql = @"SELECT Rating FROM ProductRating WHERE UserId=@UserId AND ProductId=@ProductId";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", rate.ProductId);
                cmd.Parameters.AddWithValue("@UserId", rate.UserId);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rating = (int)reader["Rating"];
                }
            }
            return rating;
        }

        //Compute Average rating and number of reviews for a product
        public static void UpdateProductDB(String productId)
        {
            double avg = 0.0;
            int num = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();

                sql = @"SELECT AVG(Rating) as Average,COUNT(*) as Count FROM ProductRating  WHERE ProductId=@ProductId
                    GROUP BY ProductId";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    avg = Convert.ToDouble( reader["Average"]);
                    num=(int) reader["Count"];
                }
                conn.Close();
            }
            UpdateRatingsInDB(avg, num,productId);
        }

        //Update Average rating and number of reviews for a product
        public static void UpdateRatingsInDB(double avg,int num,string productId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = "";
                SqlCommand cmd = new SqlCommand();

                sql = @"UPDATE Product SET Rating=@Rating,NumReviews=@Num WHERE ProductId=@ProductId";
                cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Rating", avg);
                cmd.Parameters.AddWithValue("@Num", num);
                int affected = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
