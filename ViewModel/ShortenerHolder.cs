using System.ComponentModel.DataAnnotations;

namespace Shortener.ViewModel
{
    public class ShortenerHolder
    {
        [Required]
        public string Shortener { get; set; }
    }
}
