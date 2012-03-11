using System.ComponentModel.DataAnnotations;

namespace _1CW_Site.Models
{
    public class CheckModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}