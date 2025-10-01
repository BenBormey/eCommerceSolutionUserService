using eCommerce.Core.DTO;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;
using System.Security.Claims;

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

        //    var roles = !string.IsNullOrWhiteSpace(auth.Role)
        //? new[] { auth.Role }      
        //: new[] { "Customer" };


    
            var token = _jwt.GenerateToken(auth.UserId, auth.Fullname, auth.Role);

          
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
                    phone= auth.phone,
                    auth.Role
                }
            });
        }


        [HttpPut("UpdatePasswork")]

        public async Task<IActionResult> UpdatePassword(Guid? id,[FromBody] UpdatePasswordDto update)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (update.NewPassword != update.ConfirmPassword)
                return BadRequest(new { error = "NewPassword and ConfirmPassword do not match." });
            if (id == null)
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst(ClaimTypes.Name)?.Value;
                if (!Guid.TryParse(userIdClaim, out var userid))
                    return Unauthorized("Invalid cleaner identity.");


                await usersService.UpdatePassword(userid, update);
            }
            else
            {

                await usersService.UpdatePassword((Guid)id, update);
            }



            return NoContent();
        }
    }
    
}
