using BloodNET_Web.Data;
using BloodNET_Web.Models.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BloodNET_Web.Models.Repository
{
    public class ContactsRepository:IContacts
    {
        private readonly ApplicationDbContext _context;
        private SqlConnection sqlConnection;

        public ContactsRepository(ApplicationDbContext context)
        {
            _context = context;
            sqlConnection = new SqlConnection(_context.Database.GetConnectionString());

        }

        public void Add(Contacts contacts)
        {
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
