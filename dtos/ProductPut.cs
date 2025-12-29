using System.ComponentModel.DataAnnotations;

namespace ApiRest.dtos
{
    public class ProductPut
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(255)]
        public string? Description { get; set; }
        [Required]
        public int? Amount { get; set; }
        public decimal? Price { get; set; }
        [Range(0,999999)]
        public int? CategoryId { get; set; }

    }
}