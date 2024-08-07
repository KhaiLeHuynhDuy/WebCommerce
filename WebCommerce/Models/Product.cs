using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCommerce.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string? Description { get; set; }
        
        public int? CategoryID { get; set; }
        [Required]
        public double? ProductPrice { get; set; }

    }
}
