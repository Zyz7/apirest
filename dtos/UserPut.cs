using System.ComponentModel.DataAnnotations;

namespace ApiRest.dtos
{
    public class UserPut
    {
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
        [Range(0,999999)]
        public int? AddressId { get; set; }

    }
}