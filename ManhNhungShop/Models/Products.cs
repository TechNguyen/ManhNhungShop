using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManhNhungShop.Models
{
    public class Products
    {
        [Required]
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(255)]
        public string ProductName { get; set; }
        [Required]
        [Column(TypeName = "nvarchar")]
        [MaxLength(100)]
        public string ProductType { get; set; }
        [Column(TypeName = "money")]
        public decimal ProductPrice { get; set; }
        public int ProductQuanlity { get; set; }
        [Column(TypeName = "nvarchar(max)")]
        public string ProductDescrip { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ProductCreateAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime ProductUpdateAt { get; set;}

    }
}
