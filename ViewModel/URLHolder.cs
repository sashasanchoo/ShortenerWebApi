using System.ComponentModel.DataAnnotations;

namespace Shortener.ViewModel
{
    public class URLHolder
    {
        [RegularExpression(@"^https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b(?:[-a-zA-Z0-9()@:%_\+.~#?&\/=]*)$",
             ErrorMessage = "The {0} is not valid. ")]
        [Required]
        public string URL { get; set; }
    }
}
