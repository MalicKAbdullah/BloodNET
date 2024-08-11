using BloodNET_Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BloodNET_Web.Models
{
    public class MyUsers: IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }

    }
}
