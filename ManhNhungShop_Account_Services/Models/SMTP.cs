namespace ManhNhungShop_Account_Services.Models
{
    public class SMTP
    {
        public string SendAddress { get; set; }
        public string SendDisplayname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }    
        public string Host { set; get; }
        public int Port { get; set; }   
        public bool EnableSSL { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool IsBodyHTML { set; get; }
    }
}
