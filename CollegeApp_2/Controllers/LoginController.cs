using System.Drawing.Imaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CollegeApp_2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CollegeApp_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(policyName: "AllowOnlyLocalhost")]  // Program.cs de tanimladigimiz CORS yontemlerinden hangisini kullanicaksak controllere bu sekilde uyguluyoruz
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
                return BadRequest("Please provide username and password");
            }

            LoginResponseDTO response = new() { UserName = model.UserName };

            if (model.UserName == "yunus" && model.Password == "1234")
            {
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JWTSecret")); // Program.cs de " JWT Authentication Configuration " kisminda
                var tokenHandler = new JwtSecurityTokenHandler();                                // JWTSecret degerini yazdik
                var tokenDescriptior = new SecurityTokenDescriptor()
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        // JWT nin icine koyacagimiz talepler
                        // Birincisi kullanici kimligi varmi
                        new Claim(ClaimTypes.Name, model.UserName),
                        
                        //Rol
                        new Claim(ClaimTypes.Role, "Admin")
                    }),
                    Expires = DateTime.Now.AddHours(4), // Son kullanma tarihi ve 4 saat sonra suresi dolacak
                
                    // Kimlik bilgileri saglamamiz gerekiyoe
                    SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512),
                };

                var token = tokenHandler.CreateToken(tokenDescriptior);
                response.Token = tokenHandler.WriteToken(token);
            }
            else
            {
                return Ok("Invalid userbane nad password");
            }
            return Ok (response);
        }
    }

}
