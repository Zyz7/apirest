using System.ComponentModel.DataAnnotations;

namespace ApiRest.models
{
    public class Supplier
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [Phone]
        public string? Telephone { get; set; }
        [Range(0,999999)]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        
    }
}