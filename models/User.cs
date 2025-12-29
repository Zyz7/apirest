using System.ComponentModel.DataAnnotations;

namespace ApiRest.models
{
    public class User
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }
        public char? Active { get; set; }
        [Range(0,999999)]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }

    }
}