using System.Security.Claims;

namespace BloodNET_Web.Models
{
    public static class UserExtensions
    {
        public static string getUserId (this ClaimsPrincipal user) 
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value;
        }


    }


}
