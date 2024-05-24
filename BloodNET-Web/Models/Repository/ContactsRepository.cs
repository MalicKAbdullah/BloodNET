using Microsoft.Data.SqlClient;

namespace BloodNET_Web.Models.Repository
{
    public class ContactsRepository
    {
        public const string connectionstring = "Server=(localdb)\\mssqllocaldb;Database=BloodNET;Trusted_Connection=True;MultipleActiveResultSets=true";
        public ContactsRepository() { }

        public void Add(Contacts contacts)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionstring);
            string insertQuery = "INSERT INTO contacts(Name, PhoneNumber, Email, Subject,Body) VALUES(@name,@phoneNumber, @email, @subject, @body)";

            sqlConnection.Open();
            SqlCommand insertCommand = new SqlCommand(insertQuery, sqlConnection);
            insertCommand.Parameters.AddWithValue("name", contacts.Name);

            insertCommand.Parameters.AddWithValue("phoneNumber", contacts.PhoneNumber);
            insertCommand.Parameters.AddWithValue("email", contacts.Email);


            insertCommand.Parameters.AddWithValue("Subject", contacts.Subject);
            insertCommand.Parameters.AddWithValue("body", contacts.Body);

            insertCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

    }
}
