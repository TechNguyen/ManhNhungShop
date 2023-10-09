using System.ComponentModel.DataAnnotations;

namespace ManhNhungShop_Account_Services.Models
{
    public class OtpCode
    {
        public int Otpcode { set; get; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { set; get; }
    }
}
