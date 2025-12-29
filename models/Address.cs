using System.ComponentModel.DataAnnotations;

namespace ApiRest.models
{
    public class Address
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Street { get; set; }
        [Required]
        public string? Num { get; set; }
        [Required]
        [StringLength(50)]
        public string? Colony { get; set;}
        [Required]
        public int? PostalCode { get; set; }
        [Required]
        [StringLength(50)]
        public string? State { get; set; }
        [Required]
        [StringLength(50)]
        public string? Country { get; set; }

    }
}