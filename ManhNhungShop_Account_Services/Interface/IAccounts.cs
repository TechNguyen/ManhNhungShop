using ManhNhungShop_Account_Services.Models;

namespace ManhNhungShop_Account_Services.Interface
{
    public interface IAccounts
    {
        Task<Accounts> Login(LoginModel account);
        Task<string> Generate(Accounts account);
        Task<bool> CreateAccount(Accounts account);

        Task<bool> UpdateDetails(Accounts accountdetail);
    }
}
