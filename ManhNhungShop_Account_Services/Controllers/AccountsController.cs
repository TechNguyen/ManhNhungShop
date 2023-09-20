using ManhNhungShop_Account_Services.Interface;
using ManhNhungShop_Account_Services.Models;
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
        //login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginForAccount([FromBody] Accounts account)
        {
            try
            {
                var user = _accounts.Authentication(account);
                if (user == null)
                {
                    
                }
                return Ok();
            } catch( Exception ex)
            { 
                return NotFound(ex.Message);
            }
        }
        //logout
        [HttpGet("logout")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateNew([FromBody] Accounts account)
        {
            try
            {
                return Ok();
            } catch( Exception ex )
            {
                return NotFound(ex.Message);
            }
        }
        //create a new account
        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateNewAccount([FromBody] Accounts account)
        {
            try
            {
                return Ok();
            } catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        //update detail information account
        [HttpPost("detail")]
        [Authorize(Roles = "Administrator,Customer")]
        public async Task<IActionResult> UpdateDetailCustomer([FromBody] AccountsDetails accountdetail)
        {
            try
            {
                var isUpdate = _accounts.UpdateDetails(accountdetail);
                return Ok();
            } catch( Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
            
    }
}
