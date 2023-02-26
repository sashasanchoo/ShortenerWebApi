using System.ComponentModel.DataAnnotations;

namespace Shortener.ViewModel
{
    public class AuthenticationRequest
    {
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
