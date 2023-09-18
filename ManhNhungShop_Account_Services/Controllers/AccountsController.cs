using ManhNhungShop_Account_Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManhNhungShop_Account_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccounts _accounts;
        private readonly ILogger _logger;

        private IConfiguration _configuration;
   

        public AccountsController(ILogger<AccountsController> logger, IAccounts accounts, IConfiguration configuration)
        {
            _accounts = accounts;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> SignInForAccount(string username, string password)
        {
            try
            {
                var user = Authenticaton()
                return Ok();
            } catch( Exception ex)
            { 
                return NotFound();
            }
        }
    }
}
