using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManhNhungShop_Account_Services.Models
{
    public class LoginModel
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        [Key]
        public string UserName { set; get; }
        [Required]
        [MinLength(6)]
        [Column(TypeName = "varchar(30)")]
        public string Password { set; get; }
        [Required]
        [MinLength(6)]
        [Column(TypeName = "varchar(30)")]
        public string ConfirmPassword { set; get; }
    }
}
