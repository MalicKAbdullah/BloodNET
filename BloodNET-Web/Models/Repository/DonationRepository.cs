using Microsoft.Data.SqlClient;

namespace BloodNET_Web.Models.Repository
{
    public class DonationRepository
    {
        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";


        public DonationRepository() { }

        public List<(string,DateTime)> GetDonors(int reqId)
        {
            var Donorlist = new List<(string,DateTime)>();
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            string selectQuery = "SELECT * FROM Donation where Requestid = @rid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("rid", reqId);

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                (string, DateTime) obj;
                obj.Item1 = sqlDataReader["donorId"].ToString();
                obj.Item2 = DateTime.Parse(sqlDataReader["Created"].ToString());
                Donorlist.Add(obj);
            }

            sqlConnection.Close();
            return Donorlist;
        }

    }
}
