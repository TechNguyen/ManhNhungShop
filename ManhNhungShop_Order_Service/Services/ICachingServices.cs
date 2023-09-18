namespace ManhNhungShop_Order_Service.Services
{
    public interface ICachingServices
    {
        T GetData<T>(string key);
        // set key
        bool SetData<T>(string key, T data, DateTimeOffset expireTime);
        //remove
        object RemoveData(string key);
        //refresh expireTime
        Task<bool> RefreshData(string key);
        //check data
        Task<bool> UpdateData<T>(ICollection<T> datacahing, ICollection<T> data);

    }
}
