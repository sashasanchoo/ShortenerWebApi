using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Shortener.Model
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [RegularExpression(@"[a-zA-Z0-9._]+", ErrorMessage = "The {0} must contain notning except alphabetic characters, digits, \".\" or \"_\" symbols")]
        [DisplayName("username")]
        public override string UserName { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Display(Name = "password")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Display(Name = "confirm password")]

        public string ConfirmPassword { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "The {0} can not be empty. ")]
        [Display(Name = "email")]
        public override string Email { get; set; }
    }
}
