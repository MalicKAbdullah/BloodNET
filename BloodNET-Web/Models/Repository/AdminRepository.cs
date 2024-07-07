using BloodNET_Web.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using static Dapper.SqlMapper;

namespace BloodNET_Web.Models.Repository
{ 
    [Authorize(Policy = "adminPolicy")]
    public class AdminRepository : IAdmin
    {
        public readonly string connectionString = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";

        public List<MyUsers> GetUsers()
        {
           
            var query = "select * from AspNetUsers ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<MyUsers>(query).ToList();
            }

        }

        public List<Donation> GetDonations()
        {

            var query = "select * from Donation ";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.Query<Donation>(query).ToList();
            }

        }

        public void DeleteUser(string id)
        {
    

            var query = $"delete from AspNetUsers where id = '{id}'";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
