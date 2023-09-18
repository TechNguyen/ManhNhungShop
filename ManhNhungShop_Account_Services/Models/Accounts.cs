using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManhNhungShop_Account_Services.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    public class Accounts
    {
        [Required]
        [Column(TypeName = "string(50)")]
        [Key]
        public string UserName { set; get; }
        [Required]
        [MinLength(6)]
        [Column(TypeName = "string(10)")]
        public string Password { set; get; }
    }
}
