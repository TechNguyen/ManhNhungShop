namespace ManhNhungShop_Account_Services.Interface
{
    public interface IAccounts
    {
        Task<bool> Authentication(string username, string password);
        Task<bool> Generate(string username);
        Task<bool> Verify(string username);
    }
}
