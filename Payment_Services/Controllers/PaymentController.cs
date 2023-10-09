using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payment_Services.Interface;

namespace Payment_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment _payment;
        public PaymentController(IPayment payment) { 
            _payment = payment; 
        }
        [HttpGet("")]
        public async Task<IActionResult> Payment()
        {
            try
            {
                var result = _payment.CreateNewPayement();
                return Ok();
            } catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
