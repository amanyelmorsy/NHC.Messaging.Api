using Microsoft.AspNetCore.Mvc;
using NHC.Messaging.Application.Services;

namespace NHC.Messaging.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _customerService.GetAll();
            return Ok(result);
        }
    }
}
