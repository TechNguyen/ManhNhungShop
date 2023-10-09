using Microsoft.Identity.Client;

namespace ManhNhungShop_Account_Services.Models
{
    public class UserOptionMail
    {
        public string toEmail { set; get; }
        public string? subject { set; get; }

        public string body { set; get; }

        public List<KeyValuePair<string,int>> keyValuePairs { set; get; }
    }
}
