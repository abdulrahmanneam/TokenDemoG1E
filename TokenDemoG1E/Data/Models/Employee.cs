using Microsoft.AspNetCore.Identity;

namespace TokenDemoG1E.Data.Models
{
    public class Employee : IdentityUser
    {
        public string Department { get; set; }
    }
}
