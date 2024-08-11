using BloodNET_Web.Data;
using BloodNET_Web.Models.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace BloodNET_Web.Models.Repository
{
    public class UsersRepository
    {
        private static SqlConnection sqlConnection = new SqlConnection("Data Source=ABDULLAH-MALIK\\SQLEXPRESS;Initial Catalog=BloodNET;Integrated Security=True;TrustServerCertificate=True;");

        public static string GetImgUrl(string id)
        {
            string selectQuery = "SELECT * FROM BloodDonors where donorid = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            string url = String.Empty;

            while (sqlDataReader.Read())
            {
                url = sqlDataReader["ImgUrl"].ToString();
            }

            sqlConnection.Close();
            return url;
        }

        public static (string name, string email) GetNameandEmail(string id)
        {
            string selectQuery = "SELECT * FROM AspNetUsers where id = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            string name = String.Empty;
            string email = string.Empty;

            while (sqlDataReader.Read())
            {
                name = sqlDataReader["FirstName"].ToString() + " " + sqlDataReader["LastName"].ToString();
                email = sqlDataReader["Email"].ToString();
            }

            sqlConnection.Close();
            return (name, email);
        }


    }
}
