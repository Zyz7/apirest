using System.ComponentModel.DataAnnotations;

namespace ApiRest.dtos
{
    public class Password
    {
        [Required]
        [MinLength(8)]
        public string? Current { get; set; }
        [Required]
        [MinLength(8)]
        public string? New { get; set; }
        [Required]
        [MinLength(8)]
        [Compare("New")]
        public string? Confirm { get; set; }

    }
}