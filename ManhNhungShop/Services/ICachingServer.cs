namespace ManhNhungShop.Services
{
    public interface ICachingServer
    {
        // getData
        T GetData<T>(string key);
        // set key
        bool SetData<T>(string key, T data, DateTimeOffset expireTime);
        //remove
        object RemoveData(string key);
        //refresh expireTime
        Task<bool> RefreshData(string key);

    }
}
