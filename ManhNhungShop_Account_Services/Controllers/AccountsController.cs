using ManhNhungShop_Account_Services.Interface;
using ManhNhungShop_Account_Services.Models;
using ManhNhungShop_Account_Services.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

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
        public async Task<IActionResult> LoginForAccount([FromBody] LoginModel accountlogin)
        {
            try
            {
                var user = await _accounts.Login(accountlogin);
                if (user != null) {
                    var token = _accounts.Generate(user);
                    return Ok(token);
                } else
                {
                    return NotFound("UserName or Password not correct");
                }
            } catch( Exception ex)
            { 
                return NotFound(ex.Message);
            }
        }
        //logout
        [HttpGet("logout")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Logout([FromBody] Accounts account)
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
                var result = await _accounts.CreateAccount(account);
                if(result == true)
                {
                    AccountCreate accountres = new AccountCreate() {
                        statusCode = 200,
                        status = true,
                        message = $"Create {account.UserName} successfiully!"
                    
                    };
                        return Ok(accountres);
                } else
                {
                    AccountCreate accountres = new AccountCreate()
                    {
                        statusCode = 409,
                        status = false,
                        message = $"Error when create new {account.UserName}"
                    };
                    return Ok(accountres);
                }
            } catch(Exception ex)
            {
                AccountCreate accountres = new AccountCreate()
                {
                    statusCode = 401,
                    status = false,
                    message = ex.Message

                };
                return NotFound(accountres);
            }
        }
        //update detail information account
        [HttpPost("detail")]
        [Authorize(Roles = "Administrator,Customer")]
        public async Task<IActionResult> UpdateDetailCustomer([FromBody] Accounts accountdetail)
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
