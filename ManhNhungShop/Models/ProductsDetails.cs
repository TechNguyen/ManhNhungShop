
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace ManhNhungShop.Models
{
    public class ProductsDetails
    {
        [Required]
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string productName { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string productDescription { get; set; }
        [Required]
        [Column(TypeName = "varchar(max)")]
        public string productImage { get; set; }
        [Column(TypeName = "ncarchar(100)")]
        public string productType { get; set; }
        [Required]
        [Column(TypeName = "nvarchar(50)")]   
        public string productOrginal { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string productColor { get; set; }
        [Column(TypeName = "varchar(50)")]
        public string productPackage { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public decimal productFloat { get; set; }
        public int productQuanlity { get; set; }
    }
}
