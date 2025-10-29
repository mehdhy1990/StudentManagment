using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpPost]
        public ActionResult Login(LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("please give us username and password");
            }

            LoginResponseDTO response = new() { UserName = model.UserName };
            if (model.UserName == "mehdi" && model.Password == "mehdi")
            {
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecretForLocals"));
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        //Username
                        new Claim(ClaimTypes.Name, model.UserName),
                        //Role
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.Now.AddHours(4),
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                response.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return BadRequest("username and password is not valid");
            }

            return Ok(response);
        }
    }

   

  
}
