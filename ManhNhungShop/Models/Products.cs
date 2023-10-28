using Amazon.S3.Model;
using ManhNhungShop_Product_Service.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; 

namespace ManhNhungShop.Models
{
    public class Products
    {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int? ProductId { get; set; }
            [Required]
            [Column(TypeName = "nvarchar(255)")]
            public string ProductName { get; set; }
            [Required]
            [Column(TypeName = "nvarchar(100)")]
            public string ProductType { get; set; }
            [Column(TypeName = "money")]
            public decimal? ProductPrice { get; set; }
            [Column(TypeName = "int")]
            public int? ProductQuanlity { get; set; }
            [Column(TypeName = "nvarchar(max)")]
            public string? ProductDescrip { get; set; }
            [Column(TypeName = "datetime")]
            public DateTime? ProductCreateAt { get; set; } = DateTime.Now;
            [Column(TypeName = "datetime")]
            public DateTime? ProductUpdateAt { get; set; }      
            [Column(TypeName = "varchar(max)")]
            public string? ProductImage { get; set; }
            public int? deleted { get; set; } = 0;

            public int  CategoriesId { set; get; }

            public DateTime? getdatecreate()
            {
                return this.ProductCreateAt;

            }
            public int? getdeleted()
            {
                return this.deleted;
            }
    }
}
