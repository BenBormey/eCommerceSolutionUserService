using eCommerce.Core.DTO;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IjwtRepository _jwt;
        private readonly IUsersService usersService;
        public AuthController(IUsersService user, IjwtRepository jwt)
        {
            this.usersService = user;
            this._jwt = jwt;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (model == null)
            {
                return BadRequest("Invalid registration data");
            }
            AuthenticationResponse? authentication = await usersService.Register(model);
            if (authentication == null || authentication.Success == false)
            {
                return BadRequest(authentication);
            }
            return Ok(authentication);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (login is null || string.IsNullOrWhiteSpace(login.Email) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest(new { title = "Invalid email or password" });

            var auth = await usersService.Login(login);
            if (auth is null || auth.Success == false)
                return Unauthorized(new { title = "Invalid email or password" });

            var roles = !string.IsNullOrWhiteSpace(auth.Role)
        ? new[] { auth.Role }       // wrap single string as array
        : new[] { "Customer" };


            // GenerateToken returns STRING
            var token = _jwt.GenerateToken(auth.UserId, auth.Fullname, roles.ToString());

            // If you want expiresAt but service doesn't return it, compute from config:
            var expiresAt = DateTime.UtcNow.AddHours(1);

            return Ok(new
            {
                token,
                expiresAt,
                user = new
                {
                    id = auth.UserId,
                    fullName = auth.Fullname,
                    email = auth.Email,
                    roles
                }
            });
        }


    }
    
}
