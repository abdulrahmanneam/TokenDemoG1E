using System.ComponentModel.DataAnnotations;

namespace TokenDemoG1E.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
