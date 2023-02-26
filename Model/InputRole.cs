using System.ComponentModel.DataAnnotations;

namespace Shortener.Model
{
    public class InputRole
    {
        [Required]
        public string RoleName { get; set; }
    }
}
