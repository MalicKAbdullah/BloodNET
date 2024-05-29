using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace BloodNET_Web.Models
{
    public class MyUsers: IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }

        public static (string name,string email) GetNameandEmail(string id)
        {
            const string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string selectQuery = "SELECT * FROM AspNetUsers where id = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            MyUsers user = new MyUsers();

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

        public static string GetImgUrl(string id)
        {
            const string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string selectQuery = "SELECT * FROM BloodDonors where donorid = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            MyUsers user = new MyUsers();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            string url = String.Empty;

            while (sqlDataReader.Read())
            {
                url = sqlDataReader["ImgUrl"].ToString();
            }

            sqlConnection.Close();
            return url;
        }
    }
}
