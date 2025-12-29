using System.ComponentModel.DataAnnotations;

namespace ApiRest.dtos
{
    public class UserPost
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
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }

    }
}