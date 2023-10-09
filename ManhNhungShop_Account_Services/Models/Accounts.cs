using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManhNhungShop_Account_Services.Models
{
    [Index(nameof(UserName), IsUnique = true)]
    public class Accounts
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        [Key]
        public string UserName { set; get; }
        [Required]
        [MinLength(6)]
        [Column(TypeName = "varchar(300)")]
        public string Password { set; get; }
        [Required]
        [EmailAddress]
        [Column(TypeName = "varchar(100)")]
        public string Email { set; get; }
        [Required]
        public string FirstName { set; get; }
        [Required]
        public string LastName { set; get; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime Dob { set; get;}
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string PhoneNumber { set; get; }
        [Required]
        [Column(TypeName = "varchar(300)")]
        public string ConfirmPassword { set; get; }
        [Required]
        [Column(TypeName = "varchar(30)")]
        [DefaultValue("Customer")]
        public string Role { set; get; }
    }
}
