using eCommerce.Core.DTO;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService usersService;
        public AuthController(IUsersService user)
        {
            this.usersService = user;
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
            return Ok(authentication);

        }
    }
}
