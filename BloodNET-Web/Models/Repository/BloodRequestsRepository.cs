using Microsoft.Data.SqlClient;

namespace BloodNET_Web.Models.Repository
{
    public class BloodRequestsRepository
    {
        public const string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
        public BloodRequestsRepository() { }

        public void Add(BloodRequests bloodRequests)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string insertQuery = "INSERT INTO BloodRequests(BloodGroup,DateTime,RecipientName,RecipientPhone,Location,Description) VALUES(@bgroup,@dtime,@rname,@rphone,@loc,@desc)";
            sqlConnection.Open();
            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);

            insertCommand.Parameters.AddWithValue("bgroup", bloodRequests.BloodGroup);
            insertCommand.Parameters.AddWithValue("dtime", bloodRequests.DTime);
            insertCommand.Parameters.AddWithValue("rname", bloodRequests.RecipientName);
            insertCommand.Parameters.AddWithValue("rphone", bloodRequests.RecipientPhone);
            insertCommand.Parameters.AddWithValue("loc", bloodRequests.Location);
            insertCommand.Parameters.AddWithValue("desc", bloodRequests.Description);

            int rowsAffected = insertCommand.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public void Update(BloodRequests bloodRequests)
        {

        }

        public void Delete(BloodRequests bloodRequests)
        {

        }
        public BloodRequests Get(int id)
        {
            return null;
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
                BloodRequests bloodRequest = new BloodRequests(sqlDataReader["bloodgroup"].ToString(), DateTime.Parse(sqlDataReader["datetime"].ToString()), sqlDataReader["recipientname"].ToString(), sqlDataReader["recipientphone"].ToString(), sqlDataReader["Location"].ToString(), sqlDataReader["description"].ToString());
                bloodRequests.Add(bloodRequest);
            }

            sqlConnection.Close();
            return bloodRequests;
        }



    }
}
