using eCommerce.Core.DTO.Customer;
using eCommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            string role = "Customer";
            var result = await usersService.GetAllCustomer(role);
            return Ok(result);
        }
        [HttpGet("GetCustomerById/{userId:guid}")]
        public async Task<IActionResult> GetCustomerById(Guid userId, string role)
        {
            role = "Customer";
            var result = await usersService.GetCustomerById(userId, role);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpGet("GetCleaner")]
        
        public async Task<IActionResult> GetCleaner()
        {
            string role = "Cleaner";
            var result = await usersService.GetAllCustomer(role);
            return Ok(result);
        }
        [HttpGet("GetCleanerById/{userId:guid}")]
        public async Task<IActionResult> GetCleanerById(Guid userId, string role)
        {
            role = "Cleaner";
            var result = await usersService.GetCustomerById(userId, role);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpDelete("DeleteCustomer/{userId:guid}")]
        [Authorize(Roles = "admin")]
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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCustomer(Guid custId, [FromBody] eCommerce.Core.DTO.Customer.EditCustomer customer)
        {
            if (customer == null || custId == Guid.Empty)
            {
                return BadRequest("Invalid customer data");
            }
            var result = await usersService.UpdateCustomer( custId, customer);
            if (!result)
            {
                return NotFound();
            }
            return Ok(new { message = "Customer updated successfully" });
        }
        //[HttpPut("updateCustomer")]
        //public async Task<IActionResult> UpdateCustomer(Guid custId,EditCustomer dto)
        //{
        //    if(dto == null)
        //    {
        //        return BadRequest("Invalid customer data");
        //    }
        //      await usersService.UpdateCustomer(custId,dto);
        //    return NoContent();
           

        //}
   

    }
}
