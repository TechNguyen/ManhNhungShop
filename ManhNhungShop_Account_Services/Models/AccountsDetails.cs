using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManhNhungShop_Account_Services.Models
{
    public class AccountsDetails
    {
        [Required]
        [Key]
        public string UserId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(200)")]
        public string Address { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string City { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string PhoneNumber { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Role { get; set; }  
    }
}
