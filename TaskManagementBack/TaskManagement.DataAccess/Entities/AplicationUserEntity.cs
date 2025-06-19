using Microsoft.AspNetCore.Identity;

namespace TaskManagement.DataAccess.Entities
{
    public class AplicationUserEntity : IdentityUser
    {
        public string FullName { get; set; }

    }
}
