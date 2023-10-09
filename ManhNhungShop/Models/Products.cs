using ManhNhungShop_Product_Service.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 

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
        [JsonIgnore]
        [Column(TypeName = "datetime")]
        private DateTime ProductCreateAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        [Column(TypeName = "datetime")]
        private DateTime? ProductUpdateAt { get; set; }
        [JsonIgnore]        
        [Column(TypeName = "varchar(max)")]
        public string? ProductImage { get; set; }
        public FileUpload file { get; set; }

    }
}
