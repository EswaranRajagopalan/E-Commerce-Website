using ASP.NET_Team_1.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_Team_1.Data
{
    public class SessionData : Data
    {
        public static void SessionInsert(Session session)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"insert into session (SessionId) Values (@sessId)";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@sessId", session.SessionId);
                int noAffectedRows = cmd.ExecuteNonQuery();
            }
        }

        public static void updateSession(string sessionId, User user)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"update session
                                set UserId = @UserId, Timestamp ='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "' where SessionId =@sessionId";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", user.UserID);
                cmd.Parameters.AddWithValue("@sessionId", sessionId);

                int noAffectedRows = cmd.ExecuteNonQuery();

                //Move items from temporary cart to cart
                CartData.MoveToCart(sessionId, user.UserID);
            }
        }

        public static void deleteSession(string sessionId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string sql = @"delete from session where SessionId = '" + sessionId + "'";

                SqlCommand cmd = new SqlCommand(sql, conn);

                int noAffectedRows = cmd.ExecuteNonQuery();

            }
        }
    }
}
