using System.Security.Claims;

namespace TJM103.Models
{
    internal class UserModel : ClaimsPrincipal
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }
}