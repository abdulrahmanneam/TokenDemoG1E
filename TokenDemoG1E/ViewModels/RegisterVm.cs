using System.ComponentModel.DataAnnotations;

namespace TokenDemoG1E.Controllers
{
    public class RegisterVm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
        public string Department { get; set; }
    }
}
