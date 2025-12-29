using System.ComponentModel.DataAnnotations;

namespace ApiRest.dtos
{
    public class SupplierPost
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? Telephone { get; set; }
        
    }
}