using Dapper;
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

        public List<(string donorId, int reqId)> GetDonations(string donorId)
        {
            var DonationList = new List<(string, int)>();
            string selectQuery = "SELECT * FROM Donation where donorid = @donorId";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                List<Donation> donations = sqlConnection.Query<Donation>(selectQuery, new { donorId }).ToList();

                foreach (var donation in donations)
                {
                    (string,int) obj = (donation.DonorId, donation.RequestId);
                    DonationList.Add(obj);
                }
            }
            return DonationList;
        }



    }
}
