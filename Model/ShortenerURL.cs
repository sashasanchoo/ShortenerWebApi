using System.ComponentModel.DataAnnotations;

namespace Shortener.Model
{
    public class ShortenerURL
    {
        public int Id { get; set; }
        
        public string CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public string URL { get; set; }
        public string Shortener { get; set; }
    }
}
