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
    public class LoginData : Data
    {
        public static User getUserAuthentication(string userId, string password)
        {
            User user = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string sql = @"Select * from [User] where UserId=@UserId and password =@Password COLLATE SQL_Latin1_General_CP1_CS_AS";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    user = new User()
                    {
                        Password = (string)reader["Password"],
                        UserID = (string)reader["UserId"]
                    };
                }

                conn.Close();

                return user;
            }
        }
    }
}
