using System.ComponentModel.DataAnnotations;
using ApiRest.models;

namespace ApiRest.dtos
{
    public class ProductGet
    {
        [Key]
        public int? Id { get; set; }
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
        public Category? Category { get; set; }

    }
}