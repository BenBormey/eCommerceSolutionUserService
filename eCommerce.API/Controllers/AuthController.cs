using eCommerce.Core.DTO;
using eCommerce.Core.RepositoryContracts;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
            if(model == null)
            {
                return BadRequest("Invalid registration data");
            }
      AuthenticationResponse? authentication =      await usersService.Register(model);
            if (authentication == null || authentication.Sucess ==false)
            {
                return BadRequest(authentication);
            }
            return Ok(authentication);
        }
        [HttpPost("Login")]
       public async Task<IActionResult> Login(LoginRequest login)
        {
            if(login == null)
            {
                return BadRequest("Invalid email or password");

            }
            AuthenticationResponse? authentication = await usersService.Login(login);

            if(authentication == null || authentication.Sucess ==false)
            {
                return Unauthorized(authentication);
            }
            var token = _jwt.GenerateToken(authentication.UserId, authentication.PersonName,  "Admin" );

            return Ok(token);

        }
    }
}
