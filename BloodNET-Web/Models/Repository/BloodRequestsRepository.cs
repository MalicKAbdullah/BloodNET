using BloodNET_Web.Data;
using BloodNET_Web.Models.Interfaces;
using Dapper;
using Microsoft.AspNet.Identity;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace BloodNET_Web.Models.Repository
{
    public class BloodRequestsRepository : IBloodRequests
    {
        private readonly ApplicationDbContext _context;

        private SqlConnection sqlConnection;

        public BloodRequestsRepository(ApplicationDbContext context)
        {
            _context = context;
            sqlConnection = new SqlConnection(_context.Database.GetConnectionString());

        }

        public void Add(BloodRequests bloodRequests)
        {
            string insertQuery = "INSERT INTO BloodRequests(BloodGroup,DTime,RecipientName,RecipientPhone,Location,Description,userId) VALUES(@bgroup,@dtime,@rname,@rphone,@loc,@desc,@uId)";
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

        public List<BloodRequests> SearchByType(string type, string userId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                string searchQuery = "SELECT * FROM BloodRequests WHERE BloodGroup = @type AND userId != @userId";
  
                sqlConnection.Open();
                SqlCommand selectCommand = new SqlCommand(searchQuery, sqlConnection);
                selectCommand.Parameters.AddWithValue("userId", userId);   
                selectCommand.Parameters.AddWithValue("type", type);

                List<BloodRequests> bloodRequests = new List<BloodRequests>();

                SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["dtime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                    bloodRequests.Add(bloodRequest);
                }

                sqlConnection.Close();
                return bloodRequests;
            }
        }

        public BloodRequests GetbyId(int rid)
        {
            string selectQuery = "SELECT * FROM BloodRequests WHERE id = @uid";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", rid);

            BloodRequests bloodRequest = new BloodRequests();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["dtime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
            }

            sqlConnection.Close();
            return bloodRequest;
        }

        public void UpdateStatus(int reqId, string status)
        {
            var query = "UPDATE BloodRequests SET status = @status WHERE Id = @reqId";
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                connection.Open();
                var comm = new SqlCommand(query, connection);
                comm.Parameters.AddWithValue("status", status);
                comm.Parameters.AddWithValue("reqId", reqId);

                comm.ExecuteNonQuery();
            }
        }

        public List<BloodRequests> Get(string id)
        {
            string selectQuery = "SELECT * FROM BloodRequests WHERE userid = @uid AND status = 'True'";

            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("uid", id);

            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["dtime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }

        public List<BloodRequests> GetAll()
        {
            string selectQuery = "SELECT * FROM BloodRequests";
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);

            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["dtime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }

        public List<BloodRequests> GetAllExcept(string userId)
        {
            string selectQuery = "SELECT * FROM BloodRequests WHERE userId != @userId";
            sqlConnection.Open();
            SqlCommand selectCommand = new SqlCommand(selectQuery, sqlConnection);
            selectCommand.Parameters.AddWithValue("userId", userId);

            List<BloodRequests> bloodRequests = new List<BloodRequests>();

            SqlDataReader sqlDataReader = selectCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                BloodRequests bloodRequest = new BloodRequests(int.Parse(sqlDataReader["id"].ToString()), sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["dtime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString(), sqlDataReader["userId"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }

        public void availibleRequests(List<BloodRequests> bloodRequests, List<(string donorId, int reqId)> donations)
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
