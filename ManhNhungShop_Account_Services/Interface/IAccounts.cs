using ManhNhungShop_Account_Services.Models;

namespace ManhNhungShop_Account_Services.Interface
{
    public interface IAccounts
    {
        Task<Accounts> Authentication(Accounts account);
        Task<string> Generate(AccountsDetails account);
        Task<bool> CreateAccount(Accounts account, string ConfirmPassword);

        Task<bool> UpdateDetails(AccountsDetails accountdetail);
    }
}
