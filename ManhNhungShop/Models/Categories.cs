using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManhNhungShop_Product_Service.Models
{
    public class Categories
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set;}
        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string name { get; set; }
        [Column(TypeName = "int")]
        [Required]
        public int parentId { get; set; } = 0;
    }
}
