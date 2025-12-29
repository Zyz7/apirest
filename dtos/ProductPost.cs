using System.ComponentModel.DataAnnotations;

namespace ApiRest.dtos
{
    public class ProductPost
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(255)]
        public string? Description { get; set; }
        [Required]
        public int? Amount { get; set; }
        [Required]
        public decimal? Price { get; set; }

    }
}