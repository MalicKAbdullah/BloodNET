using Microsoft.AspNetCore.Identity;

namespace BloodNET_Web.Models
{
    public class MyUsers: IdentityUser
    {
        public string FirstName {  get; set; }
        public string LastName { get; set; }
    }
}
