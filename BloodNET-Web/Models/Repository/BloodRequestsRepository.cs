using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace BloodNET_Web.Models.Repository
{
    public class BloodRequestsRepository
    {
        public const string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
        public BloodRequestsRepository() { }

        public void Add(BloodRequests bloodRequests)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string insertQuery = "INSERT INTO BloodRequests(BloodGroup,DateTime,RecipientName,RecipientPhone,Location,Description,userId) VALUES(@bgroup,@dtime,@rname,@rphone,@loc,@desc,@uId)";
            sqlConnection.Open();
            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);

            insertCommand.Parameters.AddWithValue("bgroup", bloodRequests.BloodGroup);
            insertCommand.Parameters.AddWithValue("dtime", bloodRequests.DTime);
            insertCommand.Parameters.AddWithValue("rname", bloodRequests.RecipientName);
            insertCommand.Parameters.AddWithValue("rphone", bloodRequests.RecipientPhone);
            insertCommand.Parameters.AddWithValue("loc", bloodRequests.Location);
            insertCommand.Parameters.AddWithValue("desc", bloodRequests.Description);
            insertCommand.Parameters.AddWithValue("uId", bloodRequests.userId);

            int rowsAffected = insertCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public List<BloodRequests> SearchByType(string type,string userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionstring))
            {
                
                string searchQuery = $"SELECT * FROM BloodRequests WHERE BloodGroup = @type and userId != '{userId}'";
                sqlConnection.Open();

                return sqlConnection.Query<BloodRequests>(searchQuery, new { type }).ToList();
            }
        }

        public void Update(BloodRequests bloodRequests)
        {

        }

        public void Delete(BloodRequests bloodRequests)
        {

        }

        public BloodRequests GetbyId(int rid)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string selectQuery = "SELECT * FROM BloodRequests where id = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", rid);

            BloodRequests bloodRequest = new BloodRequests();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {

                bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["datetime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
            }

            sqlConnection.Close();
            return bloodRequest;
        }

        public void UpdateStatus(int reqId,string status)
        {

         var query = $"update BloodRequests set status='{status}'  where Id = {reqId} ";
            using (var connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                var comm = new SqlCommand(query, connection);

                comm.ExecuteNonQuery();
            }
        }

        public List<BloodRequests> Get(string id)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string selectQuery = "SELECT * FROM BloodRequests where userid = @uid and status='True'";
            
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {

               BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()),sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["datetime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }

        public List<BloodRequests> GetAll()
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string selectQuery = "SELECT * FROM BloodRequests";
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);

            List<BloodRequests> bloodRequests = new List<BloodRequests>();
            
            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["datetime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }


        public List<BloodRequests> GetAllExcept(string userId)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string selectQuery = $"SELECT * FROM BloodRequests where userId !='{userId}'";
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);

            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["datetime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }


        public void availibleRequests(List<BloodRequests> bloodRequests, List<(string donorId,int reqId)> donations)
        {
            foreach (var item in donations)
            {
                var itemToRemove = bloodRequests.SingleOrDefault(r => r.Id == item.reqId);
                if (itemToRemove != null)
                    bloodRequests.Remove(itemToRemove);
            }

        }


    }
}
