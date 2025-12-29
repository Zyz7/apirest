using System.ComponentModel.DataAnnotations;

namespace ApiRest.models
{
    public class Log
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(50)]
        public string? Action { get; set; }
        [Required]
        [StringLength(50)]
        public string? Tabla { get; set; }
        [Required]
        [StringLength(100)]
        public string? Description { get; set; }
        [Required]
        public DateTime? TimeDate { get; set; }

    }
}