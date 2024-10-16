using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TokenDemoG1E.Data.Models;
using TokenDemoG1E.ViewModels;

namespace TokenDemoG1E.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Employee> _userManager;
        public UsersController(IConfiguration configuration , UserManager<Employee> userManager)
        {
            _config = configuration;  
            _userManager = userManager;
        }

        #region staticLogin
        [HttpPost]
        [Route("staticLogin")]
        public ActionResult Login(LoginVm loginModel)
        {
            if (loginModel.UserName != "mohamed")
                return Unauthorized();

            // Setting claims
            var claims = new List<Claim>
            {
                new Claim("Name" , loginModel.UserName),
                new Claim(ClaimTypes.NameIdentifier , "Identifier")
            };

            // setting key
            var keyString = _config.GetValue<string>("SecretKey");
            var keyBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyBytes);

            // combine secret key with alg
            var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // generate token
            var jwt = new JwtSecurityToken(
                claims : claims,
                signingCredentials : signingCredential,
                expires: DateTime.Now.AddMinutes(_config.GetValue<int>("TokenDuration")),
                notBefore : DateTime.Now
                );

            // generate token as string
            var tokenHandeler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandeler.WriteToken(jwt);

            return Ok(new
            {
                Token = tokenString , 
                Expire = DateTime.Now.AddMinutes(_config.GetValue<int>("TokenDuration"))
            });
        }
        #endregion

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult> Register(RegisterVm model)
        {
            var DbEmp = new Employee()
            {
                UserName = model.UserName,
                Department = model.Department
            };
            var res = await _userManager.CreateAsync(DbEmp,model.Password);
            if (!res.Succeeded)
                return BadRequest(res.Errors);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier , DbEmp.Id),
                new Claim(ClaimTypes.Role, "Engineer")
            };

            var claimsResult = await _userManager.AddClaimsAsync(DbEmp, claims);

            if(!claimsResult.Succeeded)
                return BadRequest(res.Errors);

            return Ok();
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> LoginDB(LoginVm model)
        {
            var emp = await _userManager.FindByNameAsync(model.UserName);

            if (!await _userManager.CheckPasswordAsync(emp, model.Password))
                return Unauthorized();

            var claims = await _userManager.GetClaimsAsync(emp);

            var keyString = _config.GetValue<string>("SecretKey");
            var keyBytes = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyBytes);

            var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredential,
                expires: DateTime.Now.AddMinutes(_config.GetValue<int>("TokenDuration")),
                notBefore: DateTime.Now
                );

            var tokenHandeler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandeler.WriteToken(jwt);

            return Ok(new
            {
                Token = tokenString,
                Expire = DateTime.Now.AddMinutes(_config.GetValue<int>("TokenDuration"))
            });
        }


    }
}
