using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IUsersService usersService;
        public CustomerController(IUsersService sercice)
        {
            this.usersService = sercice;    


        }
        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            var result = await usersService.GetAllCustomer();
            return Ok(result);
        }
        [HttpGet("GetCustomerById/{userId:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid userId)
        {
            var result = await usersService.GetCustomerById(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpDelete("DeleteCustomer/{userId:guid}")]
        public async Task<IActionResult> DeleteCustomer(Guid userId)
        {
            var result = await usersService.DeleteCustomer(userId);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Customer deleted successfully" });
        }
        [HttpPut("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer([FromBody] eCommerce.Core.DTO.Customer.CustomerDTO customer)
        {
            if (customer == null || customer.UserId == Guid.Empty)
            {
                return BadRequest("Invalid customer data");
            }
            var result = await usersService.UpdateCustomer(customer);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Customer updated successfully" });
        }
    }
}
