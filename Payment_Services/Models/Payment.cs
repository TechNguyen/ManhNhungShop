using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payment_Services.Models
{
    public class Payment
    {
        [Key]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string AccountNumber { set; get; }
        public string CurrentCode { set; get; }
        public string AccountName {  set; get; }
        public string AccountBank {  set; get; }
    }
}
