using Dapper;
using Microsoft.Data.SqlClient;

namespace BloodNET_Web.Models.Repository
{
    public class DonationRepository
    {
        public const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
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

        public static bool getDonorStatus(string donorId)
        {
            bool status = false;

            string selectQuery = "SELECT * FROM Donation where donorid = @donorId";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                List<Donation> donations = sqlConnection.Query<Donation>(selectQuery, new { donorId }).ToList();

                foreach (var donation in donations)
                {
                    int check = donation.Status;

                    if (check == 0)
                        status = false;
                    else
                    {
                        status = true;
                        break;
                    }
                }
            }

            return status;
        }

        public void UpdateStatus(int donationId,int status)
        {

            var query = $"update BloodRequests set status={status}  where Id = {donationId} ";
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var comm = new SqlCommand(query, connection);

                comm.ExecuteNonQuery();
            }
        }

        public int getDonationId(string donorId,int reqId)
        {
            int id = 0;
            string selectQuery = $"SELECT * FROM Donation where donorid = {donorId} and RequestId = {reqId}";
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                SqlCommand sqlCommand = new SqlCommand(selectQuery, sqlConnection);

                sqlCommand.ExecuteReader();

                List<Donation> donations = new List<Donation>();

                foreach (var donation in donations)
                {
                    id = donation.Id;
                    break;
                }
            }

            return id;
        }

    }
}
